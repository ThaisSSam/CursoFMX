import axios, {
  type AxiosError,
  type AxiosInstance,
  type AxiosRequestConfig,
  type InternalAxiosRequestConfig,
} from 'axios';

const baseURL = (import.meta.env.VITE_API_URL || 'http://localhost:5050').replace(/\/+$/, '');

const pendingRequests = new Map<string, Promise<unknown>>();

const normalizeObject = (obj: unknown): unknown => {
  if (obj === null || obj === undefined) return obj;
  if (Array.isArray(obj)) return obj.map((item) => normalizeObject(item));
  if (typeof obj !== 'object' || obj instanceof FormData || obj instanceof Date) return obj;

  const registro = obj as Record<string, unknown>;
  return Object.keys(registro)
    .sort()
    .reduce<Record<string, unknown>>((acc, key) => {
      acc[key] = normalizeObject(registro[key]);
      return acc;
    }, {});
};

const generateRequestKey = (config: AxiosRequestConfig): string => {
  const method = (config.method || 'GET').toUpperCase();
  let url = config.url || '';
  const allParams: Record<string, unknown> = {};

  if (url.includes('?')) {
    const [baseUrl, queryString] = url.split('?');
    url = baseUrl;
    new URLSearchParams(queryString).forEach((value, key) => {
      allParams[key] = value;
    });
  }

  if (config.params) Object.assign(allParams, config.params as Record<string, unknown>);
  const normalizedParams = normalizeObject(allParams);
  const paramsStr = normalizedParams && typeof normalizedParams === 'object' ? JSON.stringify(normalizedParams) : '';

  let dataStr = '';
  if (config.data) {
    if (typeof config.data === 'object' && !(config.data instanceof FormData)) {
      dataStr = JSON.stringify(normalizeObject(config.data));
    } else if (config.data instanceof FormData) {
      dataStr = 'FormData';
    } else {
      dataStr = String(config.data);
    }
  }

  return `${method}_${url}_${paramsStr}_${dataStr}`;
};

const api: AxiosInstance = axios.create({
  baseURL,
  timeout: 30000,
  withCredentials: true,
});

api.interceptors.request.use((config: InternalAxiosRequestConfig) => {
  const token = localStorage.getItem('auth_token') || localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  if (config.data && !(config.data instanceof FormData)) {
    config.headers['Content-Type'] = 'application/json';
  }
  return config;
});

const originalRequest = api.request.bind(api);

api.request = function requestComDedupe<T = unknown>(config: AxiosRequestConfig): Promise<T> {
  if (config.responseType === 'blob' || config.responseType === 'arraybuffer') {
    return originalRequest(config) as Promise<T>;
  }

  const requestKey = generateRequestKey(config);

  if (pendingRequests.has(requestKey)) {
    return pendingRequests.get(requestKey) as Promise<T>;
  }

  const requestPromise = originalRequest(config)
    .then((response) => {
      pendingRequests.delete(requestKey);
      return response;
    })
    .catch((error: unknown) => {
      pendingRequests.delete(requestKey);
      throw error;
    });

  pendingRequests.set(requestKey, requestPromise);
  return requestPromise as Promise<T>;
};

(['get', 'post', 'put', 'patch', 'delete', 'head', 'options'] as const).forEach((method) => {
  api[method] = function metodoApi<T = unknown>(...args: unknown[]): Promise<T> {
    let config: AxiosRequestConfig =
      typeof args[0] === 'string'
        ? { method, url: args[0] as string }
        : { method, ...(args[0] as object) };

    if (typeof args[0] === 'string') {
      config.url = args[0] as string;
      if (method === 'get' || method === 'delete' || method === 'head' || method === 'options') {
        if (args[1]) Object.assign(config, args[1] as object);
      } else {
        config.data = args[1];
        if (args[2]) Object.assign(config, args[2] as object);
      }
    }
    return api.request<T>(config) as Promise<T>;
  };
});

api.interceptors.response.use(
  (response) => response,
  async (error: AxiosError) => {
    const ehRotaLogin = error.config?.url?.includes('/usuarios/login');

    if ((error.response?.status === 401 || error.response?.status === 403) && !ehRotaLogin) {
      localStorage.removeItem('auth_token');
      localStorage.removeItem('token');
      localStorage.removeItem('auth_user');
      pendingRequests.clear();

      window.dispatchEvent(new CustomEvent('auth:unauthorized'));
    }

    if (!error.response && (error.code === 'ERR_NETWORK' || error.code === 'ECONNABORTED')) {
      window.dispatchEvent(new CustomEvent('api:offline'));
    }

    return Promise.reject(error);
  }
);

export default api;