import api from '../config';

export type Tarefa = {
  codigo: number;
  nome: string;
  situacao: string;
  prioridade: string;
  dataCriacao: string;
  excluido: boolean;
  responsavel: {
    id: number;
    email: string;
  } | null;
}

export type CriarTarefaInput = {
  nome: string;
  prioridade: string;
  situacao: string;
  usuarioId: number;
}

export type TarefaHistorico = {
  id: number;
  tarefaId: number;
  nome: string;
  situacao: string;
  prioridade: string; 
  usuarioId: number;
  dataAlteracao: string;
  tipoAcao: 'Criar' | 'Editar' | 'Excluir' | string;
  usuarioAlteracaoId: number | null;
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
  },

  obterHistoricoPorTarefa: async (id: number): Promise<TarefaHistorico[]> => {
    try {
      const response = await api.get<TarefaHistorico[]>(`/tarefas/${id}/historico?api-version=1`);
      return response.data;
    } catch (error: any) {
      const mensagem = error.response?.data?.errors?.[0] || error.message || 'Falha ao buscar linha do tempo da tarefa.';
      throw new Error(mensagem);
    }
  },

  obterHistoricoGeral: async (pagina = 1, registrosPorPagina = 20): Promise<{ data: TarefaHistorico[], totalCount: number }> => {
    try {
      const response = await api.get(`/tarefas/historico-geral?pagina=${pagina}&registrosPorPagina=${registrosPorPagina}&api-version=1`);
      return response.data;
    } catch (error: any) {
      const mensagem = error.response?.data?.errors?.[0] || error.message || 'Falha ao carregar log de auditoria geral.';
      throw new Error(mensagem);
    }
  }
};

export default tarefaEndpoints;