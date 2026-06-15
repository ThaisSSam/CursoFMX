import api from '../config';

export interface LogoutProps {
  onLogout: () => Promise<void> | void;
}

export const loginEndpoints = {
  executarLogin: async (corpoRequest: unknown) => {
    try {
      const response = await api.post('/usuarios/login', corpoRequest);
      return { data: response.data, success: true };
    } catch (error: any) {  
      const mensagem = error.response?.data?.errors?.[0] || error.message || 'Erro ao tentar realizar o login.';
      const erroTratado = new Error(mensagem) as any;
      erroTratado.success = false;
      throw erroTratado;
    }
  },

  solicitarRecuperacao: async (login: string) => {
    try {
      const response = await api.post('/api/v1/auth/esqueci-senha', { login: login.trim() });
      return {
        success: response.data?.success ?? true,
        message: response.data?.message || 'Link de recuperação enviado com sucesso.',
      };
    } catch (error: any) {
      const respostaErro = error.response?.data;
      throw new Error(respostaErro?.errors?.[0] || error.message || 'Erro ao solicitar recuperação.');
    }
  },

  executarLogout: async () => {
    try {
      await api.post('/usuarios/logout');
      return { success: true };
    } catch (error) {
      return { success: false };
    }
  }
};
export default loginEndpoints;


export const usuarioEndpoints = {
  obterTodosUsuarios: async (): Promise<any[]> => {
    try {
      const response = await api.get('/usuarios?api-version=1');
      return response.data;
    } catch (error: any) {
      const mensagem = error.response?.data?.errors?.[0] || error.message || 'Erro ao carregar os usuários.';
      throw new Error(mensagem);
    }
  }
}