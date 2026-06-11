import { useState, useEffect } from "react";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import customToast from "@/components/CustomToast";
import { tarefaEndpoints, type CriarTarefaInput } from "@/services/endpoints/tarefas";
import api from "@/services/config";

interface UsuarioSelect {
  id: number;
  nome: string;
}

interface TarefaFormProps {
  onSucesso: () => void;
  onCancelar: () => void;
}

export default function TarefaForm({ onSucesso, onCancelar }: TarefaFormProps) {
  const [usuarios, setUsuarios] = useState<UsuarioSelect[]>([]);
  const [enviando, setEnviando] = useState(false);
  const [nome, setNome] = useState("");
  const [prioridade, setPrioridade] = useState<number>(2);
  const [situacao, setSituacao] = useState<number>(1);
  const [usuarioId, setUsuarioId] = useState<string>("");

  useEffect(() => {
    async function carregarUsuarios() {
      try {
        const response = await api.get("/usuarios?api-version=1");
        const lista = Array.isArray(response.data) ? response.data : response.data?.data || [];
        setUsuarios(lista);
      } catch (error) {
        console.error("Erro ao carregar responsáveis para o formulário", error);
      }
    }
    carregarUsuarios();
  }, []);

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();

    if (!nome.trim()) {
      customToast({
        title: "Campo Obrigatório",
        message: "Por favor, insira o título da tarefa.",
        type: "error",
        onClose: () => { },
      });
      return;
    }

    if (!usuarioId) {
      customToast({
        title: "Campo Obrigatório",
        message: "Por favor, selecione um responsável.",
        type: "error",
        onClose: () => { },
      });
      return;
    }

    const novaTarefa: CriarTarefaInput = {
      nome,
      prioridade: Number(prioridade),
      situacao: Number(situacao),
      usuarioId: Number(usuarioId),
    };

    try {
      setEnviando(false);
      setEnviando(true);

      await tarefaEndpoints.criarTarefa(novaTarefa);

      setNome("");
      setUsuarioId("");

      if (onSucesso) {
        onSucesso();
      } else {
        window.location.href = "/tarefas";
      }

    } catch (error: any) {
      const mensagemErro = error.response?.data?.errors?.[0] || error.message || "Erro ao salvar a tarefa.";

      customToast({
        title: "Erro ao Salvar",
        message: mensagemErro,
        type: "error",
        onClose: () => { },
      });
    } finally {
      setEnviando(false);
    }
  }

  return (
    <form onSubmit={handleSubmit} className="space-y-4 text-slate-200">
      <div className="space-y-1.5">
        <label className="text-xs font-semibold text-slate-400 uppercase tracking-wider">
          Título da Tarefa
        </label>
        <Input
          type="text"
          placeholder="Ex: Desenvolver Modal de Cadastro"
          value={nome}
          onChange={(e) => setNome(e.target.value)}
          className="bg-[#0f172a] border-slate-800 text-slate-100 placeholder:text-slate-600 focus-visible:ring-blue-500"
        />
      </div>

      <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
        <div className="space-y-1.5">
          <label className="text-xs font-semibold text-slate-400 uppercase tracking-wider">
            Prioridade
          </label>
          <select
            value={prioridade}
            onChange={(e) => setPrioridade(Number(e.target.value))}
            className="w-full h-10 px-3 rounded-md bg-[#0f172a] border border-slate-800 text-slate-200 focus:outline-none focus:ring-2 focus:ring-blue-500 text-sm"
          >
            <option value={1}>Baixa</option>
            <option value={2}>Média</option>
            <option value={3}>Alta / Crítica</option>
          </select>
        </div>

        <div className="space-y-1.5">
          <label className="text-xs font-semibold text-slate-400 uppercase tracking-wider">
            Situação Inicial
          </label>
          <select
            value={situacao}
            onChange={(e) => setSituacao(Number(e.target.value))}
            className="w-full h-10 px-3 rounded-md bg-[#0f172a] border border-slate-800 text-slate-200 focus:outline-none focus:ring-2 focus:ring-blue-500 text-sm"
          >
            <option value={1}>A Fazer (Em Aberto)</option>
            <option value={2}>Em Andamento</option>
            <option value={3}>Concluída</option>
          </select>
        </div>
      </div>

      <div className="space-y-1.5">
        <label className="text-xs font-semibold text-slate-400 uppercase tracking-wider">
          Responsável
        </label>
        <select
          value={usuarioId}
          onChange={(e) => setUsuarioId(e.target.value)}
          className="w-full h-10 px-3 rounded-md bg-[#0f172a] border border-slate-800 text-slate-200 focus:outline-none focus:ring-2 focus:ring-blue-500 text-sm"
        >
          <option value="" className="bg-[#131b2e] text-slate-400">
            Selecione um usuário...
          </option>
          {usuarios.map((user: any) => (
            <option
              key={user.id}
              value={user.id}
              className="bg-[#131b2e] text-slate-200 checked:bg-blue-600"
            >
              {user.nome || user.email || `Usuário ${user.id}`}
            </option>
          ))}
        </select>
      </div>

      <div className="flex justify-end gap-3 pt-4 border-t border-slate-800/60 mt-6">
        <Button
          type="button"
          variant="ghost"
          onClick={onCancelar}
          className="text-slate-400 hover:text-white hover:bg-slate-800"
        >
          Cancelar
        </Button>
        <Button
          type="submit"
          disabled={enviando}
          className="bg-blue-600 hover:bg-blue-700 text-white font-medium px-6"
        >
          {enviando ? "Salvando..." : "Criar Tarefa"}
        </Button>
      </div>
    </form>
  );
}