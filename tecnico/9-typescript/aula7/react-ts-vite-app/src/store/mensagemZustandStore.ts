import { createStore } from "zustand/vanilla";

export type MensagemZustandState = {
  mensagem: string;
  definirMensagem: (texto: string) => void;
  limparMensagem: () => void;
  contador: number;
  incrementarContador: () => void;
  decrementarContador: () => void;
};

export const mensagemZustandStore = createStore<MensagemZustandState>((set) => ({
  mensagem: "",
  definirMensagem: (texto) => set({ mensagem: texto }),
  limparMensagem: () => set({ mensagem: "" }),
  contador: 0,
  incrementarContador: () => set((estadoAnt)=> ({contador: estadoAnt.contador + 1})),
  decrementarContador: () => set((estadoAnt)=> ({contador: estadoAnt.contador - 1})),
}));
