import { useState, type ReactNode } from "react";
import { MensagemContext } from "./mensagemContext";

export function MensagemProvider({ children }: { children: ReactNode }) {
  const [mensagem, setMensagem] = useState("");
  const mostrar = true;

  return (
    <MensagemContext.Provider
      value={{
        mensagem,
        definirMensagem: (texto) => setMensagem(texto),
        limparMensagem: () => setMensagem(""),
        mostrar: () => mostrar
      }}
    >
      {children}
    </MensagemContext.Provider>
  );
}
