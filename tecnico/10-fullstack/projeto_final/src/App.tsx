import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { Button } from "@/components/Button";
import { fazerLoginSimples } from "@/services/authService";

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
      <div className="hidden lg:flex lg:col-span-3 bg-[#0b1d45] flex-col justify-center px-20 text-white">
        <h1 className="text-4xl font-bold mb-6">
          Gerencie seu time <br />
          <span className="text-blue-500">com clareza.</span>
        </h1>
        <p className="text-gray-400 mb-12 text-lg">
          Visualize, priorize e entregue tarefas com eficiência <br />— do backlog à conclusão.
        </p>

        <div className="space-y-4 max-w-sm">
          <div className="p-4 border border-gray-700 rounded-lg bg-gray-800/50">
            <h3 className="font-semibold">Kanban intuitivo</h3>
            <p className="text-sm text-gray-400">Drag and drop entre colunas</p>
          </div>
          <div className="p-4 border border-gray-700 rounded-lg bg-gray-800/50">
            <h3 className="font-semibold">5 modos de visualização</h3>
            <p className="text-sm text-gray-400">Tabela, Kanban, Calendário, Timeline</p>
          </div>
        </div>
      </div>

      {/* LADO DIREITO */}
      <div className="lg:col-span-2 flex flex-col justify-center items-center bg-[#0f172a] p-8">
        <div className="w-full max-w-xs">
          <div className="mb-8">
            <h2 className="text-xl font-bold text-white">Bem-vindo de volta</h2>
            <p className="text-sm text-gray-500">Insira suas credenciais para continuar</p>
          </div>

          <form onSubmit={handleSubmeter} className="flex flex-col gap-4">
            <div>
              <label className="text-sm font-medium mb-1 block text-gray-500">E-mail</label>
              <input
                type="email"
                className="w-full p-2 border border-gray-300 rounded focus:ring-2 focus:ring-blue-500 outline-none bg-gray-800/50 text-white"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                placeholder="nome@empresa.com.br"
                required
              />
            </div>

            <div>
              <label className="text-sm font-medium mb-1 block text-gray-500">Senha</label>
              <input
                type="password"
                className="w-full p-2 border border-gray-300 rounded focus:ring-2 focus:ring-blue-500 outline-none bg-gray-800/50 text-white"
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
                />
                Lembrar acesso
              </label>
              <button
                type="button"
                onClick={() => navigate("/forgot-password")}
                className="text-blue-500 hover:underline text-sm"
              >
                Esqueci minha senha
              </button>
            </div>

            <Button type="submit" className="w-full bg-blue-600 hover:bg-blue-700 text-white p-2 rounded">
              Entrar
            </Button>

            {statusLogin && <p className="text-center text-sm text-white mt-2">{statusLogin}</p>}
          </form>
        </div>
      </div>
    </main>
  );
}