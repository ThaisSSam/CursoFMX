import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { Button } from "@/components/Button";
import { fazerLoginSimples } from "@/services/authService";
import { Mail, Lock } from "lucide-react";

export default function App() {
  const navigate = useNavigate();
  const [email, setEmail] = useState("");
  const [senha, setSenha] = useState("");
  const [lembrarAcesso, setLembrarAcesso] = useState(false);
  const [statusLogin, setStatusLogin] = useState("");

  const handleSubmeter = async (e: React.FormEvent) => {
    e.preventDefault();
    setStatusLogin("Conectando...");

    const resultado = await fazerLoginSimples(email, senha);

    if (resultado.sucesso) {
      setStatusLogin("Logado com sucesso!");

      setTimeout(() => {
        console.log("Navegando de fato para /home...");
        navigate("/home", { replace: true });
      }, 50);

    } else {
      setStatusLogin(`Falhou: ${resultado.erro}`);
    }
  };

  return (
    <main className="grid grid-cols-1 lg:grid-cols-5 min-h-screen">

      {/* LADO ESQUERDO */}
      <div className="hidden lg:flex lg:col-span-3 bg-[#0b1d45] flex-col justify-center items-center px-20 text-white">
        <h1 className="text-4xl font-bold mb-6 text-center">
          Gerencie seu time <br />
          <span className="text-blue-500">com clareza.</span>
        </h1>
        <p className="text-gray-400 mb-12 text-lg text-center">
          Visualize, priorize e entregue tarefas com eficiência <br />— do backlog à conclusão.
        </p>

        <div className="space-y-4 max-w-sm">
          <div className="p-4 border border-gray-700 rounded-lg bg-gray-800/50 flex flex-row">
            <div className="border-0 w-12 h-12 rounded mr-3 bg-[#0e2e0f]">
            </div>
            <div className="flex flex-col">
              <h3 className="font-semibold">Kanban intuitivo</h3>
              <p className="text-sm text-gray-400">Drag and drop entre colunas</p>
            </div>
          </div>
          <div className="p-4 border border-gray-700 rounded-lg bg-gray-800/50 flex flex-row">
            <div className="border-0 w-12 h-12 rounded mr-3 bg-[#07447d]">
            </div>
            <div className="flex flex-col">
              <h3 className="font-semibold">5 modos de visualização</h3>
              <p className="text-sm text-gray-400">Tabela, Kanban, Calendário, Timeline</p>
            </div>
          </div>
          <div className="p-4 border border-gray-700 rounded-lg bg-gray-800/50 flex flex-row">
            <div className="border-0 w-12 h-12 rounded mr-3 bg-[#633405]">
            </div>
            <div className="flex flex-col">
              <h3 className="font-semibold">Dashboard em tempo real</h3>
              <p className="text-sm text-gray-400">Indicadores e métricas do time</p>
            </div>
          </div>
        </div>
      </div>

      {/* LADO DIREITO */}
      <div className="lg:col-span-2 flex flex-col justify-center items-center bg-[#0f172a] p-8">
        <div className="w-full max-w-xs">
          <div className="mb-10 text-white flex flex-row">
            <div className="border w-12 h-12 rounded mr-3 bg-white">
            </div>
            <div className="flex flex-col">
              <h2>TaskFlow</h2>
              <p>Gestão de Atividades</p>
            </div>
          </div>
          <div className="mb-8">
            <h2 className="text-xl font-bold text-white">Bem-vindo de volta</h2>
            <p className="text-sm text-gray-500">Insira suas credenciais para continuar</p>
          </div>

          <form onSubmit={handleSubmeter} className="flex flex-col gap-4">
            <label className="text-sm font-medium mb-1 block text-gray-500">E-mail</label>
            <div className="relative flex items-center">
              <span className="absolute left-2 top-2.5 text-slate-500 ">
                <Mail className="w-5" />
              </span>
              <input
                type="email"
                className="w-full p-2 border border-[#3a475c] rounded-lg focus:ring-2 focus:ring-blue-500 outline-none bg-gray-800/50 text-white pl-8"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                placeholder="nome@empresa.com.br"
                required
              />
            </div>


            <label className="text-sm font-medium mb-1 block text-gray-500">Senha</label>
            <div className="relative flex items-center">
              <span className="absolute left-2 top-2.5 text-slate-500 ">
                <Lock className="w-5" />
              </span>
              <input
                type="password"
                className="w-full p-2 border border-[#3a475c] rounded-lg focus:ring-2 focus:ring-blue-500 outline-none bg-gray-800/50 text-white pl-8"
                value={senha}
                onChange={(e) => setSenha(e.target.value)}
                required
              />
            </div>

            <div className="flex items-center justify-between text-sm">
              <label className="flex items-center gap-2 cursor-pointer text-gray-500">
                <input
                  type="checkbox"
                  checked={lembrarAcesso}
                  onChange={(e) => setLembrarAcesso(e.target.checked)}
                  className="cursor-pointer"
                />
                Lembrar acesso
              </label>
              <button
                type="button"
                onClick={() => navigate("/forgot-password")}
                className="text-blue-500 hover:underline text-sm cursor-pointer"
              >
                Esqueci minha senha
              </button>
            </div>

            <Button
              type="submit"
              className="w-full bg-[#2b71e3] hover:bg-blue-600 text-white p-2.5 rounded-lg font-semibold text-sm transition-colors mt-2 shadow-lg shadow-blue-500/10 cursor-pointer"
            >
              Entrar
            </Button>

            {statusLogin && <p className="text-center text-sm text-white mt-2">{statusLogin}</p>}
          </form>
          <div className="mt-4">
            <hr className="border-slate-600/60" />
            <div className="flex flex-row text-white gap-1 mt-4">
              <p>Precisa de acesso?</p>
              <button type="button" className="text-blue-500 hover:text-blue-400 font-medium transition-colors cursor-pointer">Fale com o administrador</button>
            </div>
          </div>
        </div>
      </div>
    </main>
  );
}