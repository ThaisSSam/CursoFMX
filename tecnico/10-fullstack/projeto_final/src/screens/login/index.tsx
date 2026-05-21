// @ts-nocheck
import React, { useState } from "react";
import { Button } from "@/components/Button";
import { fazerLoginSimples } from "./services/authService";

export default function App() {
  const [email, setEmail] = useState("");
  const [senha, setSenha] = useState("");
  const [lembrarAcesso, setLembrarAcesso] = useState("");
  const [statusLogin, setStatusLogin] = useState("");

  const handleSubmeter = async (e: React.FormEvent) => {
    e.preventDefault();
    setStatusLogin("Conectando...");

    const resultado = await fazerLoginSimples(email, senha, lembrarAcesso);

    if (resultado.sucesso) {
      setStatusLogin("Logado com sucesso! Token salvo.");
    } else {
      setStatusLogin(`Falhou: ${resultado.erro}`);
    }
  };

  return (
    <main className="grid grid-cols-3 min-h-screen items-center justify-end p-8">
      <div className="flex min-h-screen flex-col items-center justify-center p-8 col-span-2">
        <div className="flex flex-col w-70 text-center">
          <h1>
            Gerencie seu time <span>com clareza.</span>
          </h1>
          <p className="h-fit">
            Visualize, priorize e entregue tarefas com eficiência - do backlog à
            conclusão
          </p>
        </div>
      </div>
      <div className="border-l p-0 col-span-1 flex flex-col w-full max-w-md bg-white rounded-lg h-screen items-center justify-center">       
          {statusLogin && (
            <p className="mt-4 text-sm text-center font-medium text-slate-600">
              {statusLogin}
            </p>
          )}
          <div className="flex flex-col w-80 text-start">
            <h2 className="text-xl font-bold mb-4 text-start">
              Bem-vindo de volta
            </h2>
            <p>insira suas credenciais para continuar</p>
          </div>
          <form
            onSubmit={handleSubmeter}
            className="flex flex-col gap-4 items-start w-80"
          >
            <div className="w-full">
              <label className="text-sm font-medium block mb-1">E-mail</label>
              <input
                type="email"
                className="w-full p-2 border rounded"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                required
              />
            </div>

            <div className="w-full">
              <label className="text-sm font-medium block mb-1">Senha</label>
              <input
                type="password"
                className="w-full p-2 border rounded"
                value={senha}
                onChange={(e) => setSenha(e.target.value)}
                required
              />
            </div>
            <div className="flex flex-row items-center gap-2 py-1 justify-between w-80">
              <div className="flex items-center gap-1">
                <input
                  type="checkbox"
                  id="lembrar"
                  className="h-4 w-4 rounded border-gray-300 text-primary focus:ring-primary"
                  checked={lembrarAcesso}
                  onChange={(e) => setLembrarAcesso(e.target.checked)}
                />
                <label
                  htmlFor="lembrar"
                  className="text-sm font-medium select-none cursor-pointer"
                >
                  Lembrar acesso
                </label>
              </div>
              <div>
                <p>Esqueci minha senha</p>
              </div>
            </div>

            <Button type="submit" className="w-full mt-2 rounded-70">
              Entrar
            </Button>
          </form>
        </div>      
    </main>
  );
}
