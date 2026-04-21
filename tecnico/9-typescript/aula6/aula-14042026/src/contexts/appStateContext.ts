import { createContext, useContext } from "react";

export type AppStateContextType = {
  podeSelecionarEmpresa: (empresaId: number) => boolean;
};

export const AppStateContext = createContext<AppStateContextType | undefined>(
  undefined,
);

export function useAppStateContext() {
  const ctx = useContext(AppStateContext);
  if (!ctx) {
    throw new Error(
      "useAppStateContext deve ser usado dentro de AppStateProvider",
    );
  }
  return ctx;
}
