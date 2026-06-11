import api from '../config';

// 1. Exportando a interface isolada para o TypeScript não se confundir
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

export const tarefaEndpoints = {
  obterTodas: async (): Promise<Tarefa[]> => {
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
  }
};

export default tarefaEndpoints;