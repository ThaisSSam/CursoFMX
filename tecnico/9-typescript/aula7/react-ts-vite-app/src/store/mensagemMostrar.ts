import { createStore } from "zustand";

export type MensagemMostrar = {
  mensagem:string;
  mostrar: () => void;
}

export const mensagemStore = createStore<MensagemMostrar>((set) => 
  ({
    mensagem: '',
    mostrar: ()=>set({
      mensagem:'Mostrando mensagem'
    }),
  }));