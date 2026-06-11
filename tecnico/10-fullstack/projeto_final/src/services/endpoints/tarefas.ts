import api from '../config';

export type Tarefa = {
  codigo: number;
  nome: string;
  situacao: number;
  prioridade: number;
  dataCriacao: string;
  responsavel: {
    id: number;
    email: string;
  } | null;
}

export type CriarTarefaInput = {
  nome: string;
  prioridade: number;
  situacao: number;
  usuarioId: number;
}

export const tarefaEndpoints = {
  obterTodasTarefas: async (): Promise<Tarefa[]> => {
    try {
      const response = await api.get<Tarefa[]>('/tarefas?api-version=1');
      return response.data;
    } catch (error: any) {
      const mensagem = error.response?.data?.errors?.[0] || error.message || 'Erro ao carregar a listagem de tarefas.';
      console.error(mensagem);
      throw error;
    }
  },
  
  obterMetricasDashboard: async () => {
    try {
      const response = await api.get('/tarefas/dashboard-cards?api-version=1');
      return response.data;
    } catch (error: any) {
      const mensagem = error.response?.data?.errors?.[0] || error.message || 'Erro ao carregar métricas.';
      throw new Error(mensagem);
    }
  },

  criarTarefa: async (corpoRequest: CriarTarefaInput): Promise<{ success: boolean; message: string }> => {
    try {
      const response = await api.post('/tarefas?api-version=1', corpoRequest);
      return response.data;
    } catch (error: any) {
      const mensagem = error.response?.data?.errors?.[0] || error.message || 'Falha ao tentar cadastrar a nova tarefa.';      
      throw new Error(mensagem);
    }
  }
};

export default tarefaEndpoints;