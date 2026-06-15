import { useEffect, useState, useMemo } from "react";
import SidebarComponent from "../../components/Sidebar";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { SlidersHorizontal, Download, Plus, Search } from "lucide-react";
import { useReactTable, getCoreRowModel, getPaginationRowModel, type PaginationState, type SortingState } from "@tanstack/react-table";

import { tarefaEndpoints } from "@/services/endpoints/tarefas";
import type { Tarefa } from "@/services/endpoints/tarefas";
import { type LogoutProps } from "@/services/endpoints/login";
import { useLocation, useNavigate } from "react-router-dom";
import CustomToast from "@/components/CustomToast";
import { BaseDataTable } from "@/contexts/BaseDataTable";
import { createTarefaColumns } from "./table/tableConfig";
import ConfirmDeleteModal from "@/components/ConfirmDeleteModal";
import ViewModal from "./modals/modalVisualizar";
import EditarModal from "./modals/modalEditar";
import { consultarTarefaData } from "@/services/tarefasSearch";

import FiltrosTarefas, { type FiltrosTarefasData } from "@/components/function/FiltrosTarefa";

export default function TarefasScreen({ onLogout }: LogoutProps) {
  const [tarefas, setTarefas] = useState<Tarefa[]>([]);
  const [carregando, setCarregando] = useState(true);
  const [excluindo, setExcluindo] = useState(false);
  const [exibirFiltros, setExibirFiltros] = useState(false);
  const [busca, setBusca] = useState("");
  const navigate = useNavigate();
  const location = useLocation();
  const [modalVisualizarAberto, setModalVisualizarAberto] = useState(false);
  const [modalEditarAberto, setModalEditarAberto] = useState(false);
  const [modalExcluirAberto, setModalExcluirAberto] = useState(false);
  const [tarefaSelecionada, setTarefaSelecionada] = useState<Tarefa | null>(null);

  const [pagination, setPagination] = useState<PaginationState>({ pageIndex: 0, pageSize: 10 });
  const [sorting, setSorting] = useState<SortingState>([]);
  const [totalLinhas, setTotalLinhas] = useState(0);

  const [filtrosAtivos, setFiltrosAtivos] = useState<FiltrosTarefasData>({
    pesquisaGenerica: "",
    situacao: [],
    prioridade: [],
    responsavelBusca: "",
    dataMinima: ""
  });

  const [toastConfig, setToastConfig] = useState<{
    title: string;
    message: string;
    type: 'success' | 'error' | 'warning' | 'info';
  } | null>(null);

  const carregarTarefas = async () => {
    try {
      setCarregando(true);

      const resultado = await consultarTarefaData(
        pagination.pageIndex + 1,
        pagination.pageSize,
        sorting as any,
        filtrosAtivos
      );

      setTarefas(resultado?.data ?? resultado?.items ?? []);
      setTotalLinhas(resultado?.totalCount ?? resultado?.totalRegistros ?? 0);

    } catch (err: any) {
      const mensagem = err.message || "Erro ao atualizar grid.";
      const disparar = (typeof CustomToast === "function") ? CustomToast : (CustomToast as any).default;
      if (typeof disparar === "function") {
        disparar({ title: "Erro de Carregamento", message: mensagem, type: "error", onClose: () => { } });
      }
    } finally {
      setCarregando(false);
    }
  };

  useEffect(() => {
    carregarTarefas();
  }, [pagination.pageIndex, pagination.pageSize, sorting, filtrosAtivos]);

  const handleVisualizarClick = (data: Tarefa) => {
    setTarefaSelecionada(data);
    setModalVisualizarAberto(true);
  };

  const handleEditarClick = (data: Tarefa) => {
    setTarefaSelecionada(data);
    setModalEditarAberto(true);
  };

  async function handleSucessoEdicao() {
    setModalEditarAberto(false);
    setTarefaSelecionada(null);
    await carregarTarefas();
  }

  const handleExcluirClick = (data: Tarefa) => {
    setTarefaSelecionada(data);
    setModalExcluirAberto(true);
  };

  const confirmarExclusaoComBanco = async () => {
    if (!tarefaSelecionada) return;

    try {
      setExcluindo(true);
      await tarefaEndpoints.excluirTarefa(tarefaSelecionada.codigo);

      setModalExcluirAberto(false);
      setTarefaSelecionada(null);

      setToastConfig({
        title: "Sucesso!",
        message: "Tarefa removida com sucesso do banco de dados.",
        type: "success"
      });

      await carregarTarefas();

    } catch (err: any) {
      const mensagemErro = err.response?.data?.errors?.[0] || err.message || "Erro ao excluir a tarefa.";

      setToastConfig({
        title: "Erro ao Excluir",
        message: mensagemErro,
        type: "error"
      });
    } finally {
      setExcluindo(false);
    }
  };

  const columns = useMemo(() =>
    createTarefaColumns(handleVisualizarClick, handleEditarClick, handleExcluirClick),
    [tarefas]);

  const table = useReactTable({
    data: tarefas,
    columns,
    pageCount: Math.ceil(totalLinhas / pagination.pageSize),
    state: {
      pagination,
      sorting
    },
    onPaginationChange: setPagination,
    onSortingChange: setSorting,
    manualPagination: true,
    manualSorting: true,
    getRowId: (row) => String(row.codigo),
    getCoreRowModel: getCoreRowModel(),
    getPaginationRowModel: getPaginationRowModel(),
  });

  if (carregando) {
    return <div className="flex h-screen items-center justify-center bg-[#090d16] text-white">Carregando tarefas do banco de dados...</div>;
  }

  return (
    <div className="text-white bg-[#0f172a] h-screen max-h-screen w-screen flex flex-row font-sans selection:bg-blue-500/30 overflow-hidden">
      {toastConfig && (
        <div className="absolute top-5 right-5 z-50 animate-in slide-in-from-top-5 duration-300">
          <CustomToast
            title={toastConfig.title}
            message={toastConfig.message}
            type={toastConfig.type}
            onClose={() => setToastConfig(null)}
          />
        </div>
      )}
      <SidebarComponent currentPath="/tarefas" onNavigate={(path) => navigate(path)} onLogout={onLogout} />

      <div className="flex flex-col flex-1 h-full min-h-0 overflow-hidden">
        <header className="flex justify-between items-center border-b border-slate-800 bg-[#0f172a] px-8 py-5 sticky top-0 z-10">
          <div className="flex items-center gap-2 text-sm text-slate-400">
            <span className="font-semibold text-slate-200 text-lg">Tarefas</span>
          </div>
          <div className="flex items-center gap-3">
            <Button variant="outline" className="border-slate-800 bg-[#131b2e] text-slate-300 hover:bg-slate-800 text-xs gap-2 h-9">
              <Download size={14} /> Exportar CSV
            </Button>
            <Button
              className="bg-blue-600 hover:bg-blue-700 text-white text-xs gap-2 h-9 font-medium rounded-lg shadow-lg shadow-blue-600/10"
              onClick={() => navigate("/tarefas/cadastro")}
            >
              <Plus size={14} /> Adicionar tarefa
            </Button>
          </div>
        </header>

        <main className="flex-1 flex flex-col min-h-0 p-6 bg-[#090d16] space-y-4 overflow-hidden">
          <div className="flex gap-3 items-center flex-shrink-0">
            <div className="relative flex-1">
              <Search className="absolute left-3 top-2.5 h-4 w-4 text-slate-500" />
              <Input
                placeholder="Buscar por título ou código... (Aperte Enter)"
                value={busca}
                onChange={(e) => setBusca(e.target.value)}
                onKeyDown={(e) => {
                  if (e.key === 'Enter') {
                    setFiltrosAtivos(prev => ({ ...prev, pesquisaGenerica: busca }));
                  }
                }}
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
            <Button 
              variant="ghost" 
              onClick={() => { 
                setBusca(""); 
                setFiltrosAtivos({ pesquisaGenerica: "", situacao: [], prioridade: [], responsavelBusca: "", dataMinima: "" }); 
              }} 
              className="text-slate-500 hover:text-slate-300 text-xs h-9"
            >
              × Limpar
            </Button>
          </div>

          <FiltrosTarefas
            isOpen={exibirFiltros}
            onClose={() => setExibirFiltros(false)}
            initialFiltros={filtrosAtivos}
            onFiltrosChange={(novosFiltros) => setFiltrosAtivos(novosFiltros)}
            onPesquisar={(novosFiltros) => setFiltrosAtivos(novosFiltros)}
          />

          <div className="rounded-xl border border-slate-800 bg-[#131b2e] flex-1 min-h-0">
            <BaseDataTable
              table={table}
              isLoading={false}
              enablePagination={true}
              enableServerSidePagination={true}
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

      <ViewModal
        isOpen={modalVisualizarAberto}
        tarefa={tarefaSelecionada}
        onClose={() => {
          setModalVisualizarAberto(false);
          setTarefaSelecionada(null);
        }}
      />

      <EditarModal
        isOpen={modalEditarAberto}
        tarefa={tarefaSelecionada}
        onClose={() => {
          setModalEditarAberto(false);
          setTarefaSelecionada(null);
        }}
        onSucesso={handleSucessoEdicao}
      />

      <ConfirmDeleteModal
        isOpen={modalExcluirAberto}
        isLoading={excluindo}
        taskCode={tarefaSelecionada?.codigo}
        onClose={() => {
          setModalExcluirAberto(false);
          setTarefaSelecionada(null);
        }}
        onConfirm={confirmarExclusaoComBanco}
      />
    </div>
  );
}