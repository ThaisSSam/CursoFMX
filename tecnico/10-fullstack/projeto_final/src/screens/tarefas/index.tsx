import { useEffect, useState, useMemo } from "react";
import SidebarComponent from "../../components/Sidebar";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Checkbox } from "@/components/ui/checkbox";
import { SlidersHorizontal, Download, Plus, Search, X } from "lucide-react";
import { useReactTable, getCoreRowModel, getPaginationRowModel } from "@tanstack/react-table";

import { tarefaEndpoints } from "@/services/endpoints/tarefas";
import type { Tarefa } from "@/services/endpoints/tarefas";
import { type LogoutProps } from "@/services/endpoints/login";
import { useNavigate } from "react-router-dom";
import customToast from "@/components/CustomToast";
import { BaseDataTable } from "@/contexts/BaseDataTable"
import { createTarefaColumns } from "./table/tableConfig";

export default function TarefasScreen({ onLogout }: LogoutProps) {
  const [tarefas, setTarefas] = useState<Tarefa[]>([]);
  const [carregando, setCarregando] = useState(true);
  const [exibirFiltros, setExibirFiltros] = useState(false);
  const [busca, setBusca] = useState("");
  const navigate = useNavigate();

  useEffect(() => {
    async function carregarTarefas() {
      try {
        const dados = await tarefaEndpoints.obterTodas();
        setTarefas(dados);
      } catch (err: any) {
        const mensagem = err.response?.data?.errors?.[0] || err.message || "Erro ao carregar a listagem de tarefas.";
        customToast({
          title: "Erro de Carregamento",
          message: mensagem,
          type: "error",
          onClose: () => { }
        });
      } finally {
        setCarregando(false);
      }
    }
    carregarTarefas();
  }, []);

  const tarefasFiltradas = useMemo(() => {
    return tarefas.filter(t =>
      t.nome.toLowerCase().includes(busca.toLowerCase()) ||
      t.codigo.toString().includes(busca)
    );
  }, [tarefas, busca]);

  const handleVisualizarClick = (data: Tarefa) => {
    customToast({ title: "Visualizar", message: `Visualizando tarefa ${data.codigo}`, type: "success", onClose: () => { } });
  };

  const handleEditarClick = (data: Tarefa) => {
    customToast({ title: "Editar", message: `Editando tarefa ${data.codigo}`, type: "success", onClose: () => { } });
  };

  const columns = useMemo(() =>
    createTarefaColumns(handleVisualizarClick, handleEditarClick),
    []);

  const table = useReactTable({
    data: tarefasFiltradas,
    columns,
    getCoreRowModel: getCoreRowModel(),
    getPaginationRowModel: getPaginationRowModel(),
    initialState: {
      pagination: {
        pageSize: 10,
      }
    }
  });

  if (carregando) {
    return <div className="flex h-screen items-center justify-center bg-[#090d16] text-white">Carregando tarefas do banco de dados...</div>;
  }

  return (
    <div className="text-white bg-[#0f172a] h-screen max-h-screen w-screen flex flex-row font-sans selection:bg-blue-500/30 overflow-hidden">
      <SidebarComponent currentPath="/tarefas" onNavigate={(path) => navigate(path)} onLogout={onLogout} />

      <div className="flex flex-col flex-1 h-full min-h-0 overflow-hidden">
        {/* HEADER */}
        <header className="flex justify-between items-center border-b border-slate-800 bg-[#0f172a] px-8 py-5 sticky top-0 z-10">
          <div className="flex items-center gap-2 text-sm text-slate-400">
            <span className="font-semibold text-slate-200 text-lg">Tarefas</span>
          </div>
          <div className="flex items-center gap-3">
            <Button variant="outline" className="border-slate-800 bg-[#131b2e] text-slate-300 hover:bg-slate-800 text-xs gap-2 h-9">
              <Download size={14} /> Exportar CSV
            </Button>
            <Button className="bg-blue-600 hover:bg-blue-700 text-white text-xs gap-2 h-9 font-medium rounded-lg shadow-lg shadow-blue-600/10">
              <Plus size={14} /> Adicionar tarefa
            </Button>
          </div>
        </header>

        <main className="flex-1 flex flex-col min-h-0 p-6 bg-[#090d16] space-y-4 overflow-hidden">
          <div className="flex gap-3 items-center flex-shrink-0">
            <div className="relative flex-1">
              <Search className="absolute left-3 top-2.5 h-4 w-4 text-slate-500" />
              <Input
                placeholder="Buscar por título ou código..."
                value={busca}
                onChange={(e) => setBusca(e.target.value)}
                className="pl-9 bg-[#131b2e] border-slate-800 text-slate-200 placeholder:text-slate-500 h-9 focus-visible:ring-blue-500/50"
              />
            </div>
            <Button
              variant="outline"
              onClick={() => setExibirFiltros(!exibirFiltros)}
              className={`border-slate-800 bg-[#131b2e] text-slate-300 hover:bg-slate-800 h-9 text-xs gap-2 ${exibirFiltros ? 'border-blue-500 text-blue-400' : ''}`}
            >
              <SlidersHorizontal size={14} /> Filtros
            </Button>
            <Button variant="ghost" onClick={() => setBusca("")} className="text-slate-500 hover:text-slate-300 text-xs h-9">× Limpar</Button>
          </div>

          {exibirFiltros && (
            <div className="bg-[#131b2e] border border-slate-800 rounded-xl p-5 grid grid-cols-1 md:grid-cols-4 gap-6 relative animate-in fade-in duration-200">
              <button onClick={() => setExibirFiltros(false)} className="absolute top-4 right-4 text-slate-500 hover:text-slate-300">
                <X size={16} />
              </button>
              <div className="space-y-2">
                <h4 className="text-xs font-semibold text-slate-400 uppercase tracking-wider">Situação</h4>
                <div className="space-y-2 pt-1">
                  {["A fazer", "Em andamento", "Concluída", "Bloqueada", "Em validação"].map((sit) => (
                    <div key={sit} className="flex items-center gap-2 text-xs text-slate-300">
                      <Checkbox id={sit} className="border-slate-700 data-[state=checked]:bg-blue-600" />
                      <label htmlFor={sit} className="cursor-pointer">{sit}</label>
                    </div>
                  ))}
                </div>
              </div>
              <div className="space-y-2">
                <h4 className="text-xs font-semibold text-slate-400 uppercase tracking-wider">Prioridade</h4>
                <div className="space-y-2 pt-1">
                  {["Média", "Alta", "Crítica"].map((prio) => (
                    <div key={prio} className="flex items-center gap-2 text-xs text-slate-300">
                      <Checkbox id={prio} className="border-slate-700 data-[state=checked]:bg-blue-600" />
                      <label htmlFor={prio} className="cursor-pointer">{prio}</label>
                    </div>
                  ))}
                </div>
              </div>
              <div className="space-y-2">
                <h4 className="text-xs font-semibold text-slate-400 uppercase tracking-wider">Responsável</h4>
                <Input placeholder="Buscar usuário..." className="bg-[#090d16] border-slate-800 text-xs h-8 text-slate-300 placeholder:text-slate-600" />
              </div>
              <div className="space-y-2">
                <h4 className="text-xs font-semibold text-slate-400 uppercase tracking-wider">Data Mínima</h4>
                <Input type="date" className="bg-[#090d16] border-slate-800 text-xs h-8 text-slate-400 dark:[color-scheme:dark]" />
              </div>
            </div>
          )}

          <div className="rounded-xl border border-slate-800 bg-[#131b2e] text-slate-900 flex-1 min-h-0 text-slate-900">
            <BaseDataTable
              table={table}
              isLoading={false}
              enablePagination={true}
              enableServerSidePagination={false}
              enableColumnPinning={true}
              enableColumnResizing={true}
              alturaAutomatica={false}
              rolagemHorizontalExterna={false}
              columnLabels={{
                codigo: 'Código',
                nome: 'Título',
                responsavel: 'Responsável',
                prioridade: 'Prioridade',
                situacao: 'Situação',
                dataCriacao: 'Data Criação'
              }}
            />
          </div>
        </main>
      </div>
    </div>
  );
}