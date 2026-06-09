import { useEffect, useState } from "react";
import SidebarComponent from "../../components/Sidebar";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Checkbox } from "@/components/ui/checkbox";
import { Badge } from "@/components/ui/badge";
import { Avatar, AvatarFallback } from "@/components/ui/avatar";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow
} from "@/components/ui/table";
import {
  SlidersHorizontal,
  Download,
  Plus,
  Search,
  X,
  MoreHorizontal,
  ChevronLeft,
  ChevronRight
} from "lucide-react";
import { tarefaEndpoints } from "@/services/endpoints/tarefas";
import type { Tarefa } from "@/services/endpoints/tarefas";
import customToast from "@/components/CustomToast";
import  { type LogoutProps }  from "@/services/endpoints/login";
import { useNavigate } from "react-router-dom";

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

  const getPrioridadeConfig = (prio: number) => {
    switch (prio) {
      case 3: return { label: "Crítica", classes: "bg-red-500/10 text-red-500 border-red-500/20" };
      case 2: return { label: "Alta", classes: "bg-orange-500/10 text-orange-500 border-orange-500/20" };
      case 1: return { label: "Média", classes: "bg-amber-500/10 text-amber-500 border-amber-500/20" };
      default: return { label: "Baixa", classes: "bg-slate-500/10 text-slate-400 border-slate-500/20" };
    }
  };

  const getSituacaoConfig = (sit: number) => {
    switch (sit) {
      case 1: return { label: "A fazer", classes: "bg-slate-700 text-slate-300 border-slate-600" };
      case 2: return { label: "Em andamento", classes: "bg-blue-500/10 text-blue-400 border-blue-500/20" };
      case 3: return { label: "Concluída", classes: "bg-emerald-500/10 text-emerald-400 border-emerald-500/20" };
      case 4: return { label: "Bloqueada", classes: "bg-rose-500/10 text-rose-500 border-rose-500/20" };
      case 5: return { label: "Em validação", classes: "bg-purple-500/10 text-purple-400 border-purple-500/20" };
      default: return { label: "Backlog", classes: "bg-slate-800 text-slate-400 border-slate-700" };
    }
  };

  const tarefasFiltradas = tarefas.filter(t =>
    t.nome.toLowerCase().includes(busca.toLowerCase()) ||
    t.codigo.toString().includes(busca)
  );

  if (carregando) {
    return <div className="flex h-screen items-center justify-center bg-[#090d16] text-white">Carregando tarefas do banco de dados...</div>;
  }

  return (
    <div className="text-white bg-[#0f172a] min-h-screen flex flex-row font-sans selection:bg-blue-500/30">
      <SidebarComponent 
        currentPath="/tarefas" 
        onNavigate={(path) => navigate(path)} 
        onLogout={onLogout} 
      />

      <div className="flex flex-col w-full overflow-y-auto max-h-screen">

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

        <main className="flex-1 p-6 bg-[#090d16] space-y-4">

          {/* PESQUISA */}
          <div className="flex gap-3 items-center">
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

          <div className="rounded-xl border border-slate-800 bg-[#131b2e] overflow-hidden">
            <Table>
              <TableHeader className="bg-[#111827]/40 border-b border-slate-800">
                <TableRow className="hover:bg-transparent border-slate-800">
                  <TableHead className="w-12 text-center"><Checkbox className="border-slate-700" /></TableHead>
                  <TableHead className="text-[11px] font-bold text-slate-400 tracking-wider uppercase w-28">Código</TableHead>
                  <TableHead className="text-[11px] font-bold text-slate-400 tracking-wider uppercase">Título</TableHead>
                  <TableHead className="text-[11px] font-bold text-slate-400 tracking-wider uppercase">Responsável</TableHead>
                  <TableHead className="text-[11px] font-bold text-slate-400 tracking-wider uppercase w-24">Prioridade</TableHead>
                  <TableHead className="text-[11px] font-bold text-slate-400 tracking-wider uppercase w-32">Situação</TableHead>
                  <TableHead className="text-[11px] font-bold text-slate-400 tracking-wider uppercase w-28">Data Criação</TableHead>
                  <TableHead className="text-[11px] font-bold text-slate-400 tracking-wider uppercase w-16 text-center">Ações</TableHead>
                </TableRow>
              </TableHeader>
              <TableBody>
                {tarefasFiltradas.length === 0 ? (
                  <TableRow>
                    <TableCell colSpan={8} className="text-center py-10 text-slate-500">
                      Nenhuma tarefa encontrada no banco de dados.
                    </TableCell>
                  </TableRow>
                ) : (
                  tarefasFiltradas.map((tarefa) => {
                    const prioridadeInfo = getPrioridadeConfig(tarefa.prioridade);
                    const situacaoInfo = getSituacaoConfig(tarefa.situacao);
                    const nomeResponsavel = tarefa.responsavel?.email ? tarefa.responsavel.email.split('@')[0] : "Sem dono";
                    const iniciais = nomeResponsavel.substring(0, 2).toUpperCase();

                    return (
                      <TableRow key={tarefa.codigo} className="border-slate-800/60 hover:bg-[#1e293b]/30 transition-colors text-xs text-slate-300">
                        <TableCell className="text-center"><Checkbox className="border-slate-700" /></TableCell>
                        <TableCell className="font-mono text-slate-500 font-medium">TASK-{tarefa.codigo.toString().padStart(4, '0')}</TableCell>
                        <TableCell className="font-medium text-slate-100 max-w-xs">{tarefa.nome}</TableCell>
                        <TableCell>
                          <div className="flex items-center gap-2">
                            <Avatar className="w-5 h-5 text-[9px] font-bold">
                              <AvatarFallback className="bg-blue-600/20 text-blue-400 font-extrabold">
                                {iniciais}
                              </AvatarFallback>
                            </Avatar>
                            <span className="text-slate-300 capitalize">{nomeResponsavel}</span>
                          </div>
                        </TableCell>
                        <TableCell>
                          <Badge variant="outline" className={`text-[10px] px-2 py-0.5 font-semibold rounded-md ${prioridadeInfo.classes}`}>
                            {prioridadeInfo.label}
                          </Badge>
                        </TableCell>
                        <TableCell>
                          <Badge variant="outline" className={`text-[10px] px-2 py-0.5 font-medium rounded-md ${situacaoInfo.classes}`}>
                            {situacaoInfo.label}
                          </Badge>
                        </TableCell>
                        <TableCell className="text-slate-400">
                          {new Date(tarefa.dataCriacao).toLocaleDateString('pt-BR')}
                        </TableCell>
                        <TableCell className="text-center">
                          <Button variant="ghost" size="icon" className="h-7 w-7 text-slate-500 hover:text-slate-300">
                            <MoreHorizontal size={14} />
                          </Button>
                        </TableCell>
                      </TableRow>
                    );
                  })
                )}
              </TableBody>
            </Table>
          </div>

          <div className="flex justify-between items-center text-xs text-slate-500 pt-2 px-1">
            <div>Exibindo {tarefasFiltradas.length} tarefas encontradas</div>
            <div className="flex items-center gap-1">
              <Button variant="outline" size="icon" className="w-7 h-7 bg-[#131b2e] border-slate-800 text-slate-400 hover:bg-slate-800"><ChevronLeft size={14} /></Button>
              <Button variant="outline" size="icon" className="w-7 h-7 bg-blue-600 text-white border-blue-600 hover:bg-blue-700">1</Button>
              <Button variant="outline" size="icon" className="w-7 h-7 bg-[#131b2e] border-slate-800 text-slate-400 hover:bg-slate-800"><ChevronRight size={14} /></Button>
            </div>
          </div>

        </main>
      </div>
    </div>
  );
}