import React, { useState, useEffect, useCallback } from 'react';
import { X } from 'lucide-react';
import { Checkbox } from "@/components/ui/checkbox";
import { Input } from "@/components/ui/input";
import { tarefaEndpoints } from "@/services/endpoints/tarefas"; 
import api from '@/services/config';
import customToast from "@/components/CustomToast";

export interface FiltrosTarefasData {
  pesquisaGenerica: string;
  situacao: string[];   
  prioridade: string[]; 
  responsavelBusca: string;
  dataMinima: string;
  [key: string]: unknown;
}

interface FiltrosTarefasProps {
  isOpen: boolean;
  onClose: () => void;
  onFiltrosChange: (filtros: FiltrosTarefasData) => void;
  onPesquisar: (filtros: FiltrosTarefasData) => void;
  initialFiltros?: FiltrosTarefasData;
}

interface OpcaoFiltro {
  id: string;
  label: string;
}

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
};

export default function FiltrosTarefas({
  isOpen,
  onClose,
  onPesquisar,
  initialFiltros
}: Omit<FiltrosTarefasProps, 'onFiltrosChange'> & { onFiltrosChange?: any }) {

  const [filtros, setFiltros] = useState<FiltrosTarefasData>(() => {
    return initialFiltros || {
      pesquisaGenerica: '',
      situacao: [],
      prioridade: [],
      responsavelBusca: '',
      dataMinima: ''
    };
  });

  const [opcoesSituacao, setOpcoesSituacao] = useState<OpcaoFiltro[]>([]);
  const [opcoesPrioridade, setOpcoesPrioridade] = useState<OpcaoFiltro[]>([]);
  const [opcoesUsuarios, setOpcoesUsuarios] = useState<OpcaoFiltro[]>([]);

  const exibirToastErro = (titulo: string, mensagem: string) => {
    const disparar = (typeof customToast === "function") ? customToast : (customToast as any).default;
    
    if (typeof disparar === "function") {
      disparar({ title: titulo, message: mensagem, type: "error", onClose: () => { } });
    } else if ((customToast as any).error) {
      (customToast as any).error({ title: titulo, message: mensagem });
    }
  };

  const carregarDicionariosDoBackend = useCallback(async () => {
    if (opcoesSituacao.length > 0 || opcoesPrioridade.length > 0 || opcoesUsuarios.length > 0) {
      return;
    }

    try {
      const [situacoesDoBanco, prioridadesDoBanco, respostaUsuarios] = await Promise.all([
        tarefaEndpoints.obterOpcoesSituacao(),
        tarefaEndpoints.obterOpcoesPrioridade(),
        usuarioEndpoints.obterTodosUsuarios()
      ]);

      if (situacoesDoBanco.length > 0) setOpcoesSituacao(situacoesDoBanco);
      if (prioridadesDoBanco.length > 0) setOpcoesPrioridade(prioridadesDoBanco);

      const dadosUsuarios = (respostaUsuarios as any)?.data ?? (respostaUsuarios as any)?.dados ?? respostaUsuarios ?? [];
      const listaNormalizada = Array.isArray(dadosUsuarios) ? dadosUsuarios : [];

      setOpcoesUsuarios(listaNormalizada.map((u: any) => ({
        id: String(u.id ?? u.Id),
        label: u.nome ?? u.Nome ?? u.email ?? u.Email ?? `Usuário ${u.id}`
      })));

    } catch (error: any) {
      const mensagemErro = error.response?.data?.errors?.[0] || error.message || "Falha ao carregar dicionários de filtros.";
      exibirToastErro("Erro de Carregamento", mensagemErro);
    }
  }, [opcoesSituacao.length, opcoesPrioridade.length, opcoesUsuarios.length]);

  useEffect(() => {
    carregarDicionariosDoBackend();
  }, [carregarDicionariosDoBackend]); 

  useEffect(() => {
    if (initialFiltros) {
      setFiltros(initialFiltros);
    }
  }, [initialFiltros]);

  const aplicarMudancaFiltro = (novosFiltros: FiltrosTarefasData) => {
    setFiltros(novosFiltros);
    onPesquisar(novosFiltros);
  };

  const handleCheckboxChange = (campo: 'situacao' | 'prioridade', id: string, checked: boolean) => {
    const listaAtual = filtros[campo];
    const novaLista = checked ? [...listaAtual, id] : listaAtual.filter(item => item !== id);
    const novosFiltros = { ...filtros, [campo]: novaLista };
    aplicarMudancaFiltro(novosFiltros);
  };

  if (!isOpen) return null;

  return (
    <div className="bg-[#131b2e] border border-slate-800 rounded-xl p-5 grid grid-cols-1 md:grid-cols-4 gap-6 relative animate-in fade-in duration-200">
      
      <button 
        type="button" 
        onClick={onClose} 
        className="absolute top-4 right-4 text-slate-500 hover:text-slate-300 transition-colors cursor-pointer"
      >
        <X size={16} />
      </button>

      <div className="space-y-2">
        <h4 className="text-xs font-semibold text-slate-400 uppercase tracking-wider">Situação</h4>
        <div className="space-y-2 pt-1">
          {opcoesSituacao.map((sit) => (
            <div key={sit.id} className="flex items-center gap-2 text-xs text-slate-300">
              <Checkbox 
                id={`sit-${sit.id}`} 
                className="border-slate-700 data-[state=checked]:bg-blue-600"
                checked={filtros.situacao.includes(sit.id)}
                onCheckedChange={(checked) => handleCheckboxChange('situacao', sit.id, !!checked)}
              />
              <label htmlFor={`sit-${sit.id}`} className="cursor-pointer select-none">{sit.label}</label>
            </div>
          ))}
        </div>
      </div>

      <div className="space-y-2">
        <h4 className="text-xs font-semibold text-slate-400 uppercase tracking-wider">Prioridade</h4>
        <div className="space-y-2 pt-1">
          {opcoesPrioridade.map((prio) => (
            <div key={prio.id} className="flex items-center gap-2 text-xs text-slate-300">
              <Checkbox 
                id={`prio-${prio.id}`} 
                className="border-slate-700 data-[state=checked]:bg-blue-600"
                checked={filtros.prioridade.includes(prio.id)}
                onCheckedChange={(checked) => handleCheckboxChange('prioridade', prio.id, !!checked)}
              />
              <label htmlFor={`prio-${prio.id}`} className="cursor-pointer select-none">{prio.label}</label>
            </div>
          ))}
        </div>
      </div>

      <div className="space-y-2">
        <h4 className="text-xs font-semibold text-slate-400 uppercase tracking-wider">Responsável</h4>
        <select
          value={filtros.responsavelBusca}
          onChange={(e) => aplicarMudancaFiltro({ ...filtros, responsavelBusca: e.target.value })}
          className="w-full bg-[#090d16] border border-slate-800 text-xs h-8 text-slate-300 rounded-md px-2 focus:outline-none focus:ring-1 focus:ring-blue-500/50 cursor-pointer"
        >
          <option value="">Todos os responsáveis</option>
          {opcoesUsuarios.map((user) => (
            <option key={user.id} value={user.id} className="bg-[#131b2e] text-white">
              {user.label}
            </option>
          ))}
        </select>
      </div>

      <div className="space-y-2">
        <h4 className="text-xs font-semibold text-slate-400 uppercase tracking-wider">Data Mínima</h4>
        <Input 
          type="date" 
          value={filtros.dataMinima}
          onChange={(e) => aplicarMudancaFiltro({ ...filtros, dataMinima: e.target.value })}
          className="bg-[#090d16] border-slate-800 text-xs h-8 text-slate-200 focus-visible:ring-blue-500/50 cursor-pointer [color-scheme:dark] [&::-webkit-calendar-picker-indicator]:cursor-pointer [&::-webkit-calendar-picker-indicator]:opacity-80 hover:[&::-webkit-calendar-picker-indicator]:opacity-100 transition-opacity"
        />
      </div>

    </div>
  );
}