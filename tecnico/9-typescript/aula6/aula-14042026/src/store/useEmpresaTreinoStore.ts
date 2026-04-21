import { useStore } from "zustand";
import { empresaTreinoStore } from "./empresaTreinoStore";

export function useEmpresaTreinoStore() {
  return useStore(empresaTreinoStore);
}
