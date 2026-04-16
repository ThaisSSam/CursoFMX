import { useStore } from "zustand";
import { mensagemStore } from "./mensagemMostrar";

export function useMostrar(){
  return useStore(mensagemStore)
}