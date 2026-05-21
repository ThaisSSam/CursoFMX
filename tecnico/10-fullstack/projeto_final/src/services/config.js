import axios from 'axios';
// import { dbUnavailableStore } from '../store/dbUnavailableStore';
// import { apiOfflineStore } from '../store/apiOfflineStore';

const rawBase = import.meta.env.VITE_API_URL || 'http://localhost:5000';
const baseURL = rawBase.replace(/\/+$/, '');

const pendingRequests = new Map();

const normalizeObject = (obj) => {
  if (obj === null || obj === undefined) {
    return obj;
  }

  if (Array.isArray(obj)) {
    return obj.map((item) => normalizeObject(item));
  }

  if (typeof obj !== 'object') {
    return obj;
  }

  if (obj instanceof FormData || obj instanceof Date) {
    return obj;
  }

  const sorted = Object.keys(obj)
    .sort()
    .reduce((acc, key) => {
      acc[key] = normalizeObject(obj[key]);
      return acc;
    }, {});

  return sorted;
};

const generateRequestKey = (config) => {
  const method = (config.method || 'get').toUpperCase();
  let url = config.url || '';

  if (url && !url.startsWith('http')) {
    if (!url.startsWith('/')) {
      url = '/' + url;
    }
  }

  let allParams = {};

  if (url.includes('?')) {
    const [baseUrl, queryString] = url.split('?');
    url = baseUrl;
    const urlParams = new URLSearchParams(queryString);
    urlParams.forEach((value, key) => {
      allParams[key] = value;
    });
  }

  if (config.params) {
    if (typeof config.params === 'object') {
      Object.assign(allParams, config.params);
    }
  }

  const normalizedParams = normalizeObject(allParams);
  const paramsStr =
    Object.keys(normalizedParams).length > 0 ? JSON.stringify(normalizedParams) : '';

  let dataStr = '';
  if (config.data) {
    if (typeof config.data === 'object' && !(config.data instanceof FormData)) {
      const normalizedData = normalizeObject(config.data);
      dataStr = JSON.stringify(normalizedData);
    } else if (config.data instanceof FormData) {
      dataStr = 'FormData';
    } else {
      dataStr = String(config.data);
    }
  }

  return `${method}_${url}_${paramsStr}_${dataStr}`;
};

const api = axios.create({
  baseURL,
  timeout: 30000,
});

api.interceptors.request.use((config) => {
  const ehRequisicaoRefresh = config.url?.includes?.('auth/refresh');
  if (!ehRequisicaoRefresh) {
    const token = localStorage.getItem('auth_token') || localStorage.getItem('token');
    if (token) config.headers.Authorization = `Bearer ${token}`;
  }
  if (config.data && !(config.data instanceof FormData)) {
    config.headers['Content-Type'] = 'application/json';
  }

  return config;
});

const originalRequest = api.request.bind(api);

api.request = function (config) {
  if (config.responseType === 'blob' || config.responseType === 'arraybuffer') {
    return originalRequest(config);
  }

  const requestKey = generateRequestKey(config);

  if (pendingRequests.has(requestKey)) {
    return pendingRequests.get(requestKey);
  }

  const requestPromise = originalRequest(config)
    .then((response) => {
      pendingRequests.delete(requestKey);
      return response;
    })
    .catch((error) => {
      pendingRequests.delete(requestKey);
      throw error;
    });

  pendingRequests.set(requestKey, requestPromise);
  return requestPromise;
};

['get', 'post', 'put', 'patch', 'delete', 'head', 'options'].forEach((method) => {
  api[method] = function (...args) {
    const config =
      typeof args[0] === 'string' ? { method, url: args[0] } : { method, ...(args[0] || {}) };

    if (typeof args[0] === 'string') {
      config.url = args[0];
      if (method === 'get' || method === 'delete' || method === 'head' || method === 'options') {
        config.params = args[1]?.params || args[1];
      } else {
        config.data = args[1];
        if (args[2]) {
          Object.assign(config, args[2]);
        }
      }
    }

    return api.request(config);
  };
});

let promessaRenovacao = null;

function limparAuthDoStorage() {
  localStorage.removeItem('auth_token');
  localStorage.removeItem('token');
  localStorage.removeItem('refresh_token');
  localStorage.removeItem('auth_expires_at');
  localStorage.removeItem('auth_user');  
}

api.interceptors.response.use(
  (response) => response,
  async (error) => {
    if (error.response?.status === 401) {
      const ehRequisicaoRefresh = error.config?.url?.includes?.('auth/refresh');
      const tokenRefresh = localStorage.getItem('refresh_token');
      const metodoEscrita = ['post', 'put', 'patch', 'delete'].includes(
        error.config.method?.toLowerCase(),
      );

      if (ehRequisicaoRefresh || !tokenRefresh || metodoEscrita) {
        limparAuthDoStorage();
        localStorage.clear();
        pendingRequests.clear();

        window.location.href = '/login';

        window.dispatchEvent(new CustomEvent('auth:unauthorized'));
        return Promise.reject(error);
      }

      try {
        if (!promessaRenovacao) {
          const { renovarTokenAcesso } = await import('./authService');
          promessaRenovacao = renovarTokenAcesso().finally(() => {
            promessaRenovacao = null;
          });
        }
        await promessaRenovacao;
        error.config.headers.Authorization = `Bearer ${localStorage.getItem('auth_token')}`;
        const retryKey = generateRequestKey(error.config);
        pendingRequests.delete(retryKey);
        return api.request(error.config);
      } catch {
        limparAuthDoStorage();
        localStorage.clear();
        pendingRequests.clear();
        window.location.href = '/login';
        window.dispatchEvent(new CustomEvent('auth:unauthorized'));
        return Promise.reject(error);
      }
    }

    if (error.response?.status === 403) {
      limparAuthDoStorage();
      localStorage.clear();
      window.location.href = '/login';
      window.dispatchEvent(new CustomEvent('auth:unauthorized'));
      return Promise.reject(error);
    }

    if (
      error.response?.status === 503 &&
      error.response?.data.message == '[BancoDeDadosIndisponivel]'
    ) {
      dbUnavailableStore.getState().setUnavailable(true);
    }

    if (!error.response && (error.code === 'ERR_NETWORK' || error.code === 'ECONNABORTED')) {
      apiOfflineStore.getState().setOffline(true);
    }

    return Promise.reject(error);
  },
);

export default api;
