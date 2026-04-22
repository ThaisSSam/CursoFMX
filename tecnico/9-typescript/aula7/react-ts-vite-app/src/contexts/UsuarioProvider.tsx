import { useState, type ReactNode } from "react";
import { UsuarioContext } from "./usuarioContext";

export const UsuarioProvider = ({ children }: { children: ReactNode }) => {
  const [nomeUsuario, setNomeUsuario] = useState("");

  const definirNome = (nome: string) => setNomeUsuario(nome);
  const limparNome = () => setNomeUsuario("");

  return (
    <UsuarioContext.Provider value={{ nomeUsuario, definirNome, limparNome }}>
      {children}
    </UsuarioContext.Provider>
  );
};