import { Dialog, DialogContent, DialogHeader, DialogTitle } from "@/components/ui/dialog";
import TarefaForm from "../cadastrar/tarefaForm";
import type { Tarefa } from "@/services/endpoints/tarefas";

interface EditTarefaModalProps {
  isOpen: boolean;
  onClose: () => void;
  onSucesso: () => Promise<void> | void;
  tarefa: Tarefa | null;
}

export default function EditTarefaModal({ isOpen, onClose, onSucesso, tarefa }: EditTarefaModalProps) {
  return (
    <Dialog open={isOpen} onOpenChange={(open) => !open && onClose()}>
      <DialogContent className="sm:max-w-[500px] border-slate-800 bg-[#131b2e] text-white">
        <DialogHeader>
          <DialogTitle className="text-lg font-bold text-slate-100">
            Editar Tarefa
          </DialogTitle>
        </DialogHeader>

        <TarefaForm
          onSucesso={onSucesso}
          onCancelar={onClose}
          tarefaParaEditar={tarefa} 
        />
      </DialogContent>
    </Dialog>
  );
}