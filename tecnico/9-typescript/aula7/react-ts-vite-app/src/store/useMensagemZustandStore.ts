import { useStore } from "zustand";
import { mensagemZustandStore } from "./mensagemZustandStore";

export function useMensagemZustandStore() {
  return useStore(mensagemZustandStore);
}
