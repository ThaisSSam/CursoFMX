import { createContext } from "react";

export type MensagemContextType = {
  mensagem: string;
  definirMensagem: (texto: string) => void;
  limparMensagem: () => void;
  mostrar:(checkbox: boolean) => void;
};

export const MensagemContext = createContext<MensagemContextType | undefined>(
  undefined,
);
