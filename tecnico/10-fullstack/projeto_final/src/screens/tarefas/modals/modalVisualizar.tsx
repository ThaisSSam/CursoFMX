import { Dialog, DialogContent, DialogHeader, DialogTitle } from "@/components/ui/dialog";
import { Button } from "@/components/ui/button";
import type { Tarefa } from "@/services/endpoints/tarefas";

interface ViewTarefaModalProps {
  isOpen: boolean;
  onClose: () => void;
  tarefa: Tarefa | null;
}

export default function ViewTarefaModal({ isOpen, onClose, tarefa }: ViewTarefaModalProps) {
  return (
    <Dialog open={isOpen} onOpenChange={(open) => !open && onClose()}>
      <DialogContent className="sm:max-w-[500px] border-slate-800 bg-[#131b2e] text-white">
        <DialogHeader>
          <DialogTitle className="text-lg font-bold text-slate-100">
            Detalhes da Tarefa (TASK-{String(tarefa?.codigo).padStart(4, '0')})
          </DialogTitle>
        </DialogHeader>

        {tarefa && (
          <div className="space-y-4 pt-2 text-sm">
            <div className="border-b border-slate-800 pb-3">
              <span className="text-xs font-semibold text-slate-400 uppercase tracking-wider block mb-1">Título</span>
              <p className="text-slate-100 text-base font-medium">{tarefa.nome}</p>
            </div>

            <div className="grid grid-cols-2 gap-4 border-b border-slate-800 pb-3">
              <div>
                <span className="text-xs font-semibold text-slate-400 uppercase tracking-wider block mb-1">Situação</span>
                <p className="text-slate-200">
                  {tarefa.situacao === 3 ? "Concluída" : tarefa.situacao === 2 ? "Em Andamento" : "A Fazer"}
                </p>
              </div>
              <div>
                <span className="text-xs font-semibold text-slate-400 uppercase tracking-wider block mb-1">Prioridade</span>
                <p className="text-slate-200">
                  {tarefa.prioridade === 3 ? "Crítica" : tarefa.prioridade === 2 ? "Alta" : "Média"}
                </p>
              </div>
            </div>

            <div>
              <span className="text-xs font-semibold text-slate-400 uppercase tracking-wider block mb-1">Responsável</span>
              <p className="text-slate-200 capitalize">
                {tarefa.responsavel?.email?.split('@')[0] || "Sem dono"}
              </p>
            </div>
          </div>
        )}
        
        <div className="flex justify-end pt-4">
          <Button onClick={onClose} className="bg-slate-800 hover:bg-slate-700 text-slate-200">
            Fechar
          </Button>
        </div>
      </DialogContent>
    </Dialog>
  );
}