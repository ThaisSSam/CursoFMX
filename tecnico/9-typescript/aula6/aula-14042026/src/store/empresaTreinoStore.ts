import { createStore } from "zustand/vanilla";

export type EmpresaTreinoState = {
  empresaId: number | null;
  nomeEmpresa: string;
  definirEmpresa: (id: number, nome: string) => void;
  limparEmpresa: () => void;
};

export const empresaTreinoStore = createStore<EmpresaTreinoState>((set) => ({
  empresaId: null,
  nomeEmpresa: "",
  definirEmpresa: (id, nome) => set({ empresaId: id, nomeEmpresa: nome }),
  limparEmpresa: () => set({ empresaId: null, nomeEmpresa: "" }),
}));
