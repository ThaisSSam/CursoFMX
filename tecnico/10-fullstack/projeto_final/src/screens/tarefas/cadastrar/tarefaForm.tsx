import { useEffect, useState } from "react";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { tarefaEndpoints } from "@/services/endpoints/tarefas";
import type { Tarefa, CriarTarefaInput } from "@/services/endpoints/tarefas";
import customToast from "@/components/CustomToast";
import { usuarioEndpoints } from "@/services/endpoints/login";

interface TarefaFormProps {
  onSucesso: () => Promise<void> | void;
  onCancelar: () => void;
  tarefaParaEditar?: Tarefa | null;
}

export default function TarefaForm({ onSucesso, onCancelar, tarefaParaEditar }: TarefaFormProps) {
  const isEdicao = !!tarefaParaEditar;

  const [nome, setNome] = useState("");
  const [prioridade, setPrioridade] = useState("Baixa");
  const [situacao, setSituacao] = useState("Pendente");
  const [usuarioId, setUsuarioId] = useState<number>(0);
  const [salvando, setSalvando] = useState(false);
  const [usuarios, setUsuarios] = useState<any[]>([]);

  useEffect(() => {
    async function carregarUsuarios() {
      try {
        const dados = await usuarioEndpoints.obterTodosUsuarios();
        setUsuarios(dados);
      } catch (err: any) {
        const mensagemErro = err.response?.data?.errors?.[0] || err.message || "Erro ao listar responsáveis.";

        if (typeof customToast === "function") {
          customToast({ title: "Erro", message: mensagemErro, type: "error", onClose: () => { } });
        } else {
          (customToast as any).error?.({ title: "Erro", message: mensagemErro });
        }
      }
    }
    carregarUsuarios();
  }, []);

  useEffect(() => {
    if (tarefaParaEditar) {
      setNome(tarefaParaEditar.nome);
      setPrioridade(tarefaParaEditar.prioridade);
      setSituacao(tarefaParaEditar.situacao);
      setUsuarioId(tarefaParaEditar.responsavel?.id || 0);
    }
  }, [tarefaParaEditar]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    const corpoRequest: CriarTarefaInput = {
      nome,
      prioridade,
      situacao,
      usuarioId
    };

    try {
      setSalvando(true);

      if (isEdicao && tarefaParaEditar) {
        await tarefaEndpoints.atualizarTarefa(tarefaParaEditar.codigo, corpoRequest);
      } else {
        await tarefaEndpoints.criarTarefa(corpoRequest);
      }
      
      const dispararToast = (typeof customToast === "function") 
        ? customToast 
        : (customToast as any).default;

      if (typeof dispararToast === "function") {
        dispararToast({
          title: "Sucesso!",
          message: isEdicao ? "Tarefa alterada com sucesso." : "Tarefa cadastrada com sucesso.",
          type: "success",
          onClose: () => { }
        });
      }
      await onSucesso();

    } catch (err: any) {
      const mensagem = err.response?.data?.errors?.[0] || err.message || "Erro ao salvar os dados da tarefa.";
      
      const dispararToastErro = (typeof customToast === "function") 
        ? customToast 
        : (customToast as any).default;

      if (typeof dispararToastErro === "function") {
        dispararToastErro({ 
          title: "Erro ao Salvar", 
          message: mensagem, 
          type: "error", 
          onClose: () => { } 
        });
      }
    } finally {
      setSalvando(false);
    }
  };

  return (
    <form onSubmit={handleSubmit} className="space-y-4 text-slate-200">
      <div>
        <label className="text-xs text-slate-400 font-medium">Título da Tarefa</label>
        <Input value={nome} onChange={(e) => setNome(e.target.value)} className="bg-[#090d16] border-slate-800" required />
      </div>

      <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
        <div className="space-y-1.5">
          <label className="text-xs font-semibold text-slate-400 uppercase tracking-wider">
            Prioridade
          </label>
          <select
            value={prioridade}
            onChange={(e) => setPrioridade(e.target.value)}
            className="w-full h-10 px-3 rounded-md bg-[#0f172a] border border-slate-800 text-slate-200 focus:outline-none focus:ring-2 focus:ring-blue-500 text-sm cursor-pointer"
          >
            <option value="Baixa">Baixa</option>
            <option value="Media">Média</option>
            <option value="Alta">Alta / Crítica</option>
          </select>
        </div>

        <div className="space-y-1.5">
          <label className="text-xs font-semibold text-slate-400 uppercase tracking-wider">
            Situação Inicial
          </label>
          <select
            value={situacao}
            onChange={(e) => setSituacao(e.target.value)}
            className="w-full h-10 px-3 rounded-md bg-[#0f172a] border border-slate-800 text-slate-200 focus:outline-none focus:ring-2 focus:ring-blue-500 text-sm cursor-pointer"
          >
            <option value="Pendente">A Fazer (Em Aberto)</option>
            <option value="EmAndamento">Em Andamento</option>
            <option value="Concluido">Concluída</option>
          </select>
        </div>
      </div>

      <div className="space-y-1.5">
        <label className="text-xs font-semibold text-slate-400 uppercase tracking-wider">
          Responsável
        </label>
        <select
          value={usuarioId}
          onChange={(e) => setUsuarioId(Number(e.target.value))}
          className="w-full h-10 px-3 rounded-md bg-[#0f172a] border border-slate-800 text-slate-200 focus:outline-none focus:ring-2 focus:ring-blue-500 text-sm cursor-pointer"
        >
          <option value={0} className="bg-[#131b2e] text-slate-400">
            Selecione um usuário...
          </option>
          {usuarios.map((user: any) => {
            const nomeExibicao = user.email ? user.email.split('@')[0] : `Usuário ${user.id}`;

            return (
              <option
                key={user.id}
                value={user.id}
                className="bg-[#131b2e] text-slate-200 capitalize checked:bg-blue-600"
              >
                {nomeExibicao}
              </option>
            );
          })}
        </select>
      </div>

      <div className="flex justify-end gap-3 pt-4 border-t border-slate-800/60 mt-6">
        <Button
          type="button"
          variant="ghost"
          onClick={onCancelar}
          className="text-slate-400 hover:text-white hover:bg-slate-800 cursor-pointer"
        >
          Cancelar
        </Button>
        <Button type="submit" disabled={salvando} className="bg-blue-600 hover:bg-blue-700 text-white font-medium cursor-pointer">
          {salvando ? "Salvando..." : isEdicao ? "Salvar Alterações" : "Cadastrar Tarefa"}
        </Button>
      </div>
    </form>
  );
}