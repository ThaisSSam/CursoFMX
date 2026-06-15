import api from './config';
import type { FiltrosTarefasData } from '@/components/function/FiltrosTarefa';

export type OrdenacaoTabelaServico = {
  columnId?: string;
  direction?: 'asc' | 'desc' | string;
  priority?: number;
  id?: string;
  desc?: boolean;
};

const converterFiltrosParaAPI = (filtros: Partial<FiltrosTarefasData>) => {
  const filtrosAPI: any[] = [];
  const gruposAPI: any[] = [];

  if (filtros.situacao && filtros.situacao.length > 0) {
    filtrosAPI.push({
      nomeParametro: 'situacao',
      operador: filtros.situacao.length > 1 ? 'in' : 'eq',
      valores: filtros.situacao.map(s => Number(s)),
    });
  }

  if (filtros.prioridade && filtros.prioridade.length > 0) {
    filtrosAPI.push({
      nomeParametro: 'prioridade',
      operador: filtros.prioridade.length > 1 ? 'in' : 'eq',
      valores: filtros.prioridade.map(p => Number(p)),
    });
  }

  if (filtros.dataMinima && filtros.dataMinima.trim() !== '') {
    filtrosAPI.push({
      nomeParametro: 'dataCriacao',
      operador: 'gte', 
      valores: [filtros.dataMinima],
    });
  }

  if (filtros.responsavelBusca && filtros.responsavelBusca.trim() !== '') {
    const idValue = filtros.responsavelBusca.trim();
    
    filtrosAPI.push({
      nomeParametro: 'usuarioId', 
      operador: 'eq',
      valores: [isNaN(Number(idValue)) ? idValue : Number(idValue)],
    });
  }

  if (filtros.pesquisaGenerica && filtros.pesquisaGenerica.trim() !== '') {
    const termo = filtros.pesquisaGenerica.trim();

    const filtrosTexto: any[] = [
      { nomeParametro: 'nome', operador: 'contains', valores: [termo] },
    ];

    if (!isNaN(Number(termo))) {
      filtrosTexto.push({ nomeParametro: 'codigo', operador: 'eq', valores: [Number(termo)] });
    }

    gruposAPI.push({
      operadorLogico: 'OR',
      operadorInterno: 'OR',
      filtros: filtrosTexto,
      grupos: [],
    });
  }

  return { filtrosAPI, gruposAPI };
};

export const consultarTarefaData = async (
  page = 1,
  pageSize = 10,
  sortParams: OrdenacaoTabelaServico[] = [],
  filtros: Partial<FiltrosTarefasData> = {},
) => {
  const { filtrosAPI, gruposAPI } = converterFiltrosParaAPI(filtros);

  let campoOrdenacao = sortParams[0]?.columnId || 'codigo';

  if (campoOrdenacao === 'responsavel') {
    campoOrdenacao = 'Responsavel.Email';
  } else if (campoOrdenacao === 'dataCriacao') {
    campoOrdenacao = 'DataCriacao';
  } else if (campoOrdenacao === 'nome') {
    campoOrdenacao = 'Nome';
  }

  const payload = {
    grupoRaiz: {
      operadorLogico: 'AND',
      operadorInterno: 'AND',
      filtros: filtrosAPI,
      grupos: gruposAPI,
    },
    pagina: page,
    registrosPorPagina: pageSize,
    ordernarPorPadrao: sortParams[0]?.direction?.toUpperCase() || 'DESC',
    ordemParaOrdenar: campoOrdenacao,
    exportacao: false,
  };

  try {
    const response = await api.post('/tarefas/consultar?api-version=1', payload);
    return response.data; 
  } catch (error: any) {
    const mensagemErro = error.response?.data?.errors?.[0] || error.message || 'Erro ao consultar listagem de tarefas.';
    throw new Error(mensagemErro);
  }
};