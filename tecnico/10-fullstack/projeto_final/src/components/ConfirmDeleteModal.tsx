import { Dialog, DialogContent, DialogHeader, DialogTitle } from "@/components/ui/dialog";
import { Button } from "@/components/ui/button";
import { AlertTriangle } from "lucide-react";

interface ConfirmModalProps {
  isOpen: boolean;
  onClose: () => void;
  onConfirm: () => Promise<void> | void;
  isLoading?: boolean;
  taskCode?: number;
}

export default function ConfirmDeleteModal({
  isOpen,
  onClose,
  onConfirm,
  isLoading = false,
  taskCode,
}: ConfirmModalProps) {
  return (
    <Dialog open={isOpen} onOpenChange={(open) => !open && onClose()}>
      <DialogContent className="sm:max-w-[440px] border-slate-800 bg-[#131b2e] text-white p-6">
        <div className="flex flex-col items-center text-center space-y-3 pt-2">
          <div className="bg-rose-500/10 p-3 rounded-full text-rose-500 animate-pulse">
            <AlertTriangle size={28} />
          </div>

          <DialogHeader>
            <DialogTitle className="text-lg font-bold text-slate-100">
              Excluir Tarefa Permanentemente?
            </DialogTitle>
          </DialogHeader>

          <p className="text-sm text-slate-400 leading-relaxed">
            Você está prestes a deletar a{" "}
            <strong>TASK-{String(taskCode).padStart(4, "0")}</strong>.
            <br />
            Esta ação não poderá ser desfeita no banco de dados.
          </p>
        </div>

        <div className="flex justify-end gap-3 pt-5 border-t border-slate-800/60 mt-6">
          <Button
            type="button"
            variant="ghost"
            disabled={isLoading}
            onClick={onClose}
            className="text-slate-400 hover:text-white hover:bg-slate-800 text-xs cursor-pointer"
          >
            Cancelar
          </Button>
          <Button
            type="button"
            disabled={isLoading}
            onClick={onConfirm}
            className="bg-rose-600 hover:bg-rose-700 text-white text-xs font-semibold px-5 shadow-lg shadow-rose-600/10 cursor-pointer"
          >
            {isLoading ? "Excluindo..." : "Sim, Excluir"}
          </Button>
        </div>
      </DialogContent>
    </Dialog>
  );
}