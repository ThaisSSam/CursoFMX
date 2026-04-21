import { useContext } from "react";
import { MensagemContext } from "../contexts/mensagemContext";

export function useMensagem() {
  const ctx = useContext(MensagemContext);
  if (!ctx) {
    throw new Error("useMensagem deve ser usado dentro de MensagemProvider");
  }
  return ctx;  
}
