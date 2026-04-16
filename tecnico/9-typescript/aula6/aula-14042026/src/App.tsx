import type { ReactNode } from "react";
import { CabecalhoEmpresa } from "./components/CabecalhoEmpresa";
import { SelecaoEmpresa } from "./components/SelecaoEmpresa";
import { AppStateContext } from "./contexts/appStateContext";
import { AppUiContext } from "./contexts/appUiContext";

function AppStateProvider({ children }: { children: ReactNode }) {
  return (
    <AppStateContext.Provider
      value={{
        podeSelecionarEmpresa: (empresaId) => empresaId > 0,
        mostrarMensagem: (mensagem: string) => alert(mensagem),
      }}
    >
      {children}
    </AppStateContext.Provider>
  );
}

function AppUiProvider({ children }: { children: ReactNode }) {
  return (
    <AppUiContext.Provider
      value={{
        formatarEmpresaAtiva: (empresaId, nomeEmpresa) => {
          if (empresaId === null) {
            return "Nenhuma selecionada";
          }
          return `${nomeEmpresa} (id ${empresaId})`;
        },
      }}
    >
      {children}
    </AppUiContext.Provider>
  );
}

function AppContent() {
  return (
    <>
      <CabecalhoEmpresa titulo="Empresa ativa no sistema" />
      <SelecaoEmpresa subtitulo="Exemplo de props: este texto veio do AppContent." />
    </>
  );
}

export default function App() {
  return (
    <AppStateProvider>
      <AppUiProvider>
        <AppContent />
      </AppUiProvider>
    </AppStateProvider>
  );
}
