// @ts-nocheck
import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { Button } from "@/components/Button";

export default function EsqueciSenha() {
  const navigate = useNavigate();
  const [email, setEmail] = useState("");
  const [status, setStatus] = useState("");

  const handleRecuperar = async (e: React.FormEvent) => {
    e.preventDefault();
    setStatus("Enviando...");

    try {
      setStatus("Link enviado com sucesso! Verifique seu e-mail.");
    } catch (erro) {
      setStatus("Erro ao tentar enviar o e-mail.");
    }
  };

  return (
    <main className="flex min-h-screen items-center justify-center bg-[#090d16] p-4 font-sans">
      <div className="w-full max-w-md bg-[#0f172a]/60 border border-slate-800/80 rounded-2xl p-8 backdrop-blur-sm shadow-2xl flex flex-col items-center">
        
        <div className="w-12 h-12 bg-amber-500/10 border border-amber-500/30 rounded-xl flex items-center justify-center mb-4">
          <svg
            xmlns="http://www.w3.org/2000/svg"
            viewBox="0 0 24 24"
            fill="currentColor"
            className="w-6 h-6 text-amber-500"
          >
            <path fillRule="evenodd" d="M12 1.5a5.25 5.25 0 0 0-5.25 5.25v3a3 3 0 0 0-3 3v6.75a3 3 0 0 0 3 3h10.5a3 3 0 0 0 3-3v-6.75a3 3 0 0 0-3-3v-3c0-2.9-2.35-5.25-5.25-5.25Zm3.75 8.25v-3a3.75 3.75 0 1 0-7.5 0v3h7.5Z" clipRule="evenodd" />
          </svg>
        </div>

        <h2 className="text-2xl font-bold text-white mb-2 text-center">Recuperar senha</h2>
        <p className="text-sm text-slate-400/80 text-center mb-6 px-4">
          Informe seu e-mail cadastrado e enviaremos um link para redefinir sua senha.
        </p>

        <form onSubmit={handleRecuperar} className="w-full flex flex-col gap-4">
          <div>
            <label className="text-xs font-semibold text-slate-300 block mb-2">
              E-mail cadastrado <span className="text-red-500">*</span>
            </label>
            <div className="relative flex items-center">
              <span className="absolute left-3 text-slate-500">
                <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth={1.5} stroke="currentColor" className="w-5 h-5">
                  <path strokeLinecap="round" strokeLinejoin="round" d="M16.5 12a4.5 4.5 0 1 1-9 0 4.5 4.5 0 0 1 9 0Zm0 0c0 1.657 1.007 3 2.25 3S21 13.657 21 12a9 9 0 1 0-2.636 6.364M16.5 12V8.25" />
                </svg>
              </span>
              <input
                type="email"
                className="w-full p-2.5 pl-11 border border-slate-800 rounded-lg focus:ring-2 focus:ring-blue-500 outline-none bg-[#131c31] text-white text-sm transition-all"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                placeholder="seu@email.com.br"
                required
              />
            </div>
          </div>

          <Button type="submit" className="w-full bg-[#3b82f6] hover:bg-blue-600 text-white p-2.5 rounded-lg font-semibold text-sm transition-colors mt-2 shadow-lg shadow-blue-500/20">
            Enviar link de recuperação
          </Button>

          <div className="text-center text-xs text-slate-400 mt-2">
            Lembrou a senha?{" "}
            <button
              type="button"
              onClick={() => navigate("/login")}
              className="text-blue-500 hover:underline font-medium ml-1"
            >
              Voltar ao login
            </button>
          </div>

          {status && <p className="text-center text-xs text-slate-300 mt-1">{status}</p>}
        </form>

        <div className="w-full mt-6 p-4 border border-slate-800/60 rounded-xl bg-[#0f172a]/40 flex items-start gap-3">
          <span className="text-slate-400 mt-0.5">
            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth={2} stroke="currentColor" className="w-5 h-5">
              <path strokeLinecap="round" strokeLinejoin="round" d="M11.25 11.25l.041-.02a.75.75 0 111.083.984l-.04.04-.041.02a.75.75 0 01-1.083-.984l.04-.04zm0-3.499a.75.75 0 111.5 0 .75.75 0 01-1.5 0zM12 18.75A6.75 6.75 0 1012 5.25a6.75 6.75 0 000 13.5z" />
            </svg>
          </span>
          <p className="text-xs text-slate-400 leading-relaxed">
            O link será enviado para o e-mail cadastrado e expirará em <span className="font-bold text-slate-200">30 minutos</span>.
          </p>
        </div>
      </div>
    </main>
  );
}