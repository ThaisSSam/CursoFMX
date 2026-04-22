import { createContext } from "react";

interface UsuarioContextData {
  nomeUsuario: string;
  definirNome: (nome: string) => void;
  limparNome: () => void;
}

export const UsuarioContext = createContext<UsuarioContextData | undefined>(undefined);