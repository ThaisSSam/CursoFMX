import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { Button } from "@/components/Button";
import { fazerLoginSimples } from "@/services/authService";
import { Mail, Lock, ClipboardList } from "lucide-react";

interface LoginScreenProps {
  onLoginSucesso: (novoToken: string) => void;
}

export default function LoginScreen({ onLoginSucesso }: LoginScreenProps) {
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
      
      if (resultado.token) {
        onLoginSucesso(resultado.token);
      }

      setTimeout(() => {
        navigate("/home", { replace: true });
      }, 50);
    } else {
      setStatusLogin(`Falhou: ${resultado.erro}`);
    }
  };

  return (
    <main className="grid grid-cols-1 lg:grid-cols-5 min-h-screen font-sans">
      {/* LADO ESQUERDO*/}
      <div className="hidden lg:flex lg:col-span-3 bg-[#0b1d45] flex-col justify-center items-center px-20 text-white">
        <h1 className="text-4xl font-bold mb-6 text-center leading-tight">
          Gerencie seu time <br />
          <span className="text-blue-500">com clareza.</span>
        </h1>
        <p className="text-gray-400 mb-12 text-lg text-center max-w-md">
          Visualize, priorize e entregue tarefas com eficiência — do backlog à conclusão.
        </p>

        <div className="space-y-4 w-full max-w-sm">
          <div className="p-4 border border-gray-700/60 rounded-xl bg-gray-800/30 flex flex-row items-center">
            <div className="w-12 h-12 rounded-xl mr-4 bg-[#0e2e0f] flex items-center justify-center text-green-500 font-bold">✓</div>
            <div className="flex flex-col">
              <h3 className="font-semibold text-sm text-slate-200">Kanban intuitivo</h3>
              <p className="text-xs text-gray-400">Drag and drop entre colunas</p>
            </div>
          </div>
          <div className="p-4 border border-gray-700/60 rounded-xl bg-gray-800/30 flex flex-row items-center">
            <div className="w-12 h-12 rounded-xl mr-4 bg-[#07447d] flex items-center justify-center text-blue-400 font-bold">⊞</div>
            <div className="flex flex-col">
              <h3 className="font-semibold text-sm text-slate-200">5 modos de visualização</h3>
              <p className="text-xs text-gray-400">Tabela, Kanban, Calendário, Timeline</p>
            </div>
          </div>
          <div className="p-4 border border-gray-700/60 rounded-xl bg-gray-800/30 flex flex-row items-center">
            <div className="w-12 h-12 rounded-xl mr-4 bg-[#633405] flex items-center justify-center text-amber-500 font-bold">📊</div>
            <div className="flex flex-col">
              <h3 className="font-semibold text-sm text-slate-200">Dashboard em tempo real</h3>
              <p className="text-xs text-gray-400">Indicadores e métricas do time</p>
            </div>
          </div>
        </div>
      </div>

      {/* LADO DIREITO*/}
      <div className="lg:col-span-2 flex flex-col justify-center items-center bg-[#0f172a] p-8">
        <div className="w-full max-w-xs">
          <div className="mb-10 text-white flex items-center gap-3">
            <div className="w-10 h-10 bg-blue-600 rounded-xl flex items-center justify-center shadow-lg shadow-blue-500/20">
              <ClipboardList size={20} className="text-white" />
            </div>
            <div className="flex flex-col">
              <h2 className="font-bold text-sm leading-none text-slate-100">TaskFlow</h2>
              <p className="text-[10px] text-slate-500">Gestão de Atividades</p>
            </div>
          </div>

          <div className="mb-6">
            <h2 className="text-xl font-bold text-white">Bem-vindo de volta</h2>
            <p className="text-xs text-gray-500 mt-1">Insira suas credenciais para continuar</p>
          </div>

          <form onSubmit={handleSubmeter} className="flex flex-col gap-4">
            <div>
              <label className="text-xs font-semibold text-gray-400 block mb-1.5">E-mail <span className="text-red-500">*</span></label>
              <div className="relative flex items-center">
                <span className="absolute left-3 text-slate-400">
                  <Mail size={16} />
                </span>
                <input
                  type="email"
                  className="w-full p-2.5 pl-10 border border-[#3a475c] rounded-lg focus:border-blue-500 focus:ring-1 focus:ring-blue-500 outline-none bg-gray-800/40 text-white text-sm transition-all placeholder-slate-500"
                  value={email}
                  onChange={(e) => setEmail(e.target.value)}
                  placeholder="nome@email.com.br"
                  required
                />
              </div>
            </div>

            <div>
              <label className="text-xs font-semibold text-gray-400 block mb-1.5">Senha <span className="text-red-500">*</span></label>
              <div className="relative flex items-center">
                <span className="absolute left-3 text-amber-500">
                  <Lock size={16} fill="currentColor" />
                </span>
                <input
                  type="password"
                  className="w-full p-2.5 pl-10 border border-[#3a475c] rounded-lg focus:border-blue-500 focus:ring-1 focus:ring-blue-500 outline-none bg-gray-800/40 text-white text-sm transition-all placeholder-slate-600"
                  value={senha}
                  onChange={(e) => setSenha(e.target.value)}                 
                  required
                />
              </div>
            </div>

            <div className="flex items-center justify-between text-xs mt-1">
              <label className="flex items-center gap-2 cursor-pointer text-slate-400 select-none">
                <input
                  type="checkbox"
                  checked={lembrarAcesso}
                  onChange={(e) => setLembrarAcesso(e.target.checked)}
                  className="w-4 h-4 rounded border-gray-300 accent-blue-500 cursor-pointer"
                />
                Lembrar acesso
              </label>
              <button
                type="button"
                onClick={() => navigate("/forgot-password")}
                className="text-blue-500 hover:underline cursor-pointer font-medium"
              >
                Esqueci minha senha
              </button>
            </div>

            <Button
              type="submit"
              className="w-full bg-[#2b71e3] hover:bg-blue-600 text-white p-2.5 rounded-lg font-semibold text-sm transition-colors mt-2"
            >
              Entrar
            </Button>

            {statusLogin && <p className="text-center text-xs text-slate-300 mt-2">{statusLogin}</p>}
          </form>

          <div className="mt-8">
            <hr className="border-slate-800/60" />
            <div className="flex flex-row justify-center items-center text-xs text-slate-500 gap-1 mt-6">
              <p>Precisa de acesso?</p>
              <button type="button" className="text-blue-500 hover:text-blue-400 font-medium transition-colors cursor-pointer">Fale com o administrador</button>
            </div>
          </div>
        </div>
      </div>
    </main>
  );
}