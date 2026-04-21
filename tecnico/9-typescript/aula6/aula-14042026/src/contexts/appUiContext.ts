import { createContext, useContext } from "react";

export type AppUiContextType = {
  formatarEmpresaAtiva: (empresaId: number | null, nomeEmpresa: string) => string;
};

export const AppUiContext = createContext<AppUiContextType | undefined>(
  undefined,
);

export function useAppUiContext() {
  const ctx = useContext(AppUiContext);
  if (!ctx) {
    throw new Error("useAppUiContext deve ser usado dentro de AppUiProvider");
  }
  return ctx;
}
