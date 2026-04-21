import { useEmpresaTreinoStore } from "../store/useEmpresaTreinoStore";
import { useAppUiContext } from "../contexts/appUiContext";

type CabecalhoEmpresaProps = {
  titulo: string;
};

export function CabecalhoEmpresa({ titulo }: CabecalhoEmpresaProps) {
  const { empresaId, nomeEmpresa } = useEmpresaTreinoStore();
  const { formatarEmpresaAtiva } = useAppUiContext();

  return (
    <header className="empresa-header">
      <h2 className="empresa-header-titulo">{titulo}</h2>
      <p className="empresa-header-texto">
        <strong>Empresa ativa:</strong> {formatarEmpresaAtiva(empresaId, nomeEmpresa)}
      </p>
    </header>
  );
}
