import { useEffect, useState } from "react";
import { Dialog, DialogContent, DialogHeader, DialogTitle } from "@/components/ui/dialog";
import { Button } from "@/components/ui/button";
import { Calendar, Clock, User } from "lucide-react"; 
import { tarefaEndpoints, type Tarefa, type TarefaHistorico } from "@/services/endpoints/tarefas";
import { usuarioEndpoints } from "@/components/function/FiltrosTarefa";

interface ViewTarefaModalProps {
  isOpen: boolean;
  onClose: () => void;
  tarefa: Tarefa | null;
}

export default function ViewTarefaModal({ isOpen, onClose, tarefa }: ViewTarefaModalProps) {
  const [historico, setHistorico] = useState<TarefaHistorico[]>([]);
  const [carregandoHistorico, setCarregandoHistorico] = useState(false);
  const [mapaUsuarios, setMapaUsuarios] = useState<Record<string, string>>({});

  useEffect(() => {
    if (!isOpen || !tarefa?.codigo) {
      setHistorico([]);
      return;
    }

    async function buscarLogs() {
      try {
        setCarregandoHistorico(true);
        const [logs, respostaUsuarios] = await Promise.all([
          tarefaEndpoints.obterHistoricoPorTarefa(tarefa!.codigo),
          usuarioEndpoints.obterTodosUsuarios()
        ]);
        const dadosUsuarios = (respostaUsuarios as any)?.data ?? (respostaUsuarios as any)?.dados ?? respostaUsuarios ?? [];
        const listaNormalizada = Array.isArray(dadosUsuarios) ? dadosUsuarios : [];
        
        const dicionario: Record<string, string> = {};
        listaNormalizada.forEach((u: any) => {
          const id = String(u.id ?? u.Id);
          const nomeBruto = u.nome ?? u.Nome ?? u.email ?? u.Email ?? `Usuário ${id}`;
          dicionario[id] = nomeBruto.includes('@') ? nomeBruto.split('@')[0] : nomeBruto;
        });

        setMapaUsuarios(dicionario);
        setHistorico(logs)
      } catch (error: any) {
        const mensagemErro = error.response?.data?.errors?.[0] || error.message || "Erro ao obter histórico.";
        console.error("Erro capturado:", mensagemErro);
      } finally {
        setCarregandoHistorico(false);
      }
    }

    buscarLogs();
  }, [isOpen, tarefa]);

  const formatarEnumTexto = (texto: string | undefined) => {
    if (!texto) return "";
    return texto.replace(/([A-Z])/g, ' $1').trim();
  };

  return (
    <Dialog open={isOpen} onOpenChange={(open) => !open && onClose()}>
      <DialogContent className="sm:max-w-[550px] max-h-[85vh] border-slate-800 bg-[#131b2e] text-white flex flex-col overflow-hidden p-6">
        
        <DialogHeader className="flex-shrink-0">
          <DialogTitle className="text-lg font-bold text-slate-100">
            Detalhes da Tarefa (TASK-{String(tarefa?.codigo).padStart(4, '0')})
          </DialogTitle>
        </DialogHeader>

        {tarefa && (
          <div className="flex-1 overflow-y-auto space-y-6 pr-1 custom-scrollbar pt-2">
            
            <div className="bg-[#090d16]/50 border border-slate-800/80 rounded-xl p-4 space-y-4">
              <div>
                <span className="text-xs font-semibold text-slate-500 uppercase tracking-wider block mb-1">Título</span>
                <p className="text-slate-100 text-base font-medium">{tarefa.nome}</p>
              </div>

              <div className="grid grid-cols-3 gap-4">
                <div>
                  <span className="text-xs font-semibold text-slate-500 uppercase tracking-wider block mb-1">Situação</span>
                  <p className="text-slate-200 capitalize">
                    {formatarEnumTexto(tarefa.situacao)}
                  </p>
                </div>
                <div>
                  <span className="text-xs font-semibold text-slate-500 uppercase tracking-wider block mb-1">Prioridade</span>
                  <p className="text-slate-200 capitalize">
                    {formatarEnumTexto(tarefa.prioridade)}
                  </p>
                </div>
                <div>
                  <span className="text-xs font-semibold text-slate-500 uppercase tracking-wider block mb-1">Responsável</span>
                  <p className="text-slate-200 capitalize">
                    {tarefa.responsavel?.email?.split('@')[0] || "Sem dono"}
                  </p>
                </div>
              </div>
            </div>

            <div className="space-y-3">
              <h3 className="text-xs font-bold text-slate-400 uppercase tracking-wider flex items-center gap-2">
                <Calendar size={13} /> Histórico de Alterações
              </h3>

              {carregandoHistorico ? (
                <p className="text-xs text-slate-500 animate-pulse py-2">Buscando histórico...</p>
              ) : historico.length === 0 ? (
                <p className="text-xs text-slate-500 bg-[#090d16]/20 border border-dashed border-slate-800 rounded-lg p-3 text-center">
                  Nenhum registro de alteração foi encontrado para esta tarefa.
                </p>
              ) : (
                <div className="relative border-l border-slate-800 ml-3.5 pl-5 space-y-5 py-2">
                  {historico.map((log) => (
                    <div key={log.id} className="relative group text-xs">

                      <div className="space-y-1 bg-[#090d16]/30 border border-slate-800/40 rounded-lg p-3 hover:border-slate-800 transition-colors">
                        <div className="flex justify-between items-center">
                          <span className="font-bold text-slate-200 uppercase text-[10px] tracking-wide">
                            Ação: {log.tipoAcao}
                          </span>
                          <span className="text-slate-500 text-[10px] flex items-center gap-1">
                            <Clock size={10} />
                            {new Date(log.dataAlteracao).toLocaleString('pt-BR')}
                          </span>
                        </div>
                        
                        <p className="text-slate-300 font-medium text-xs pt-0.5">"{log.nome}"</p>
                        
                        <div className="grid grid-cols-2 gap-y-1.5 pt-2 text-[11px] text-slate-400 border-t border-slate-800/30 mt-1.5">
                          <span>Situação: <strong className="text-slate-300 font-normal">{log.situacao}</strong></span>
                          <span>Prioridade: <strong className="text-slate-300 font-normal">{log.prioridade}</strong></span>
                          
                          <span className="col-span-2 flex items-center gap-1 text-slate-400 pt-0.5">
                            <User size={11} className="text-slate-500" />
                            Atribuído a: <strong className="text-slate-300 font-normal capitalize">
                              {mapaUsuarios[String(log.usuarioId)] || `Usuário (ID ${log.usuarioId})`}
                            </strong>
                          </span>
                        </div>
                      </div>

                    </div>
                  ))}
                </div>
              )}
            </div>

          </div>
        )}
        
        <div className="flex justify-end pt-4 border-t border-slate-800/60 flex-shrink-0">
          <Button onClick={onClose} className="bg-slate-800 hover:bg-slate-700 text-slate-200 text-xs h-8 px-4 rounded-lg cursor-pointer">
            Fechar
          </Button>
        </div>

      </DialogContent>
    </Dialog>
  );
}