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
  },

  excluirTarefa: async (id: number): Promise<{ success: boolean; message: string }> => {
    try {
      const response = await api.delete(`/tarefas/${id}?api-version=1`);
      return response.data;
    } catch (error: any) {
      const mensagem = error.response?.data?.errors?.[0] || error.message || 'Falha ao tentar excluir a tarefa.';
      throw new Error(mensagem);
    }
  },

  atualizarTarefa: async (id: number, corpoRequest: CriarTarefaInput): Promise<{ success: boolean; message: string }> => {
    try {
      const response = await api.put(`/tarefas/${id}?api-version=1`, corpoRequest);
      return response.data;
    } catch (error: any) {
      const mensagem = error.response?.data?.errors?.[0] || error.message || 'Falha ao tentar atualizar a tarefa.';
      throw new Error(mensagem);
    }
  },

  obterOpcoesSituacao: async (): Promise<Array<{ id: string; label: string }>> => {
    try {
      const response = await api.get('/tarefas/situacoes?api-version=1');
      // Garante a extração seja o dado direto ou encapsulado em um envelope .data
      const dados = response.data?.data ?? response.data;
      return dados.map((item: any) => ({
        id: String(item.id ?? item.Id),
        label: item.label ?? item.Label
      }));
    } catch (error) {
      console.error("Falha ao buscar situações do back-end", error);
      return [];
    }
  },

  obterOpcoesPrioridade: async (): Promise<Array<{ id: string; label: string }>> => {
    try {
      const response = await api.get('/tarefas/prioridades?api-version=1');
      const dados = response.data?.data ?? response.data;
      return dados.map((item: any) => ({
        id: String(item.id ?? item.Id),
        label: item.label ?? item.Label
      }));
    } catch (error) {
      console.error("Falha ao buscar prioridades do back-end", error);
      return [];
    }
  }
};

export default tarefaEndpoints;