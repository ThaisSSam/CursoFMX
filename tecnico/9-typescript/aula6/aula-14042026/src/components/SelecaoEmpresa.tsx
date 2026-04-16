import Button from "./Button";
import { useEmpresaTreinoStore } from "../store/useEmpresaTreinoStore";
import { useAppStateContext } from "../contexts/appStateContext";

type SelecaoEmpresaProps = {
  subtitulo: string;
};

export function SelecaoEmpresa({ subtitulo }: SelecaoEmpresaProps) {
  const { definirEmpresa, limparEmpresa } = useEmpresaTreinoStore();
  const { podeSelecionarEmpresa, mostrarMensagem } = useAppStateContext();

  const selecionarEmpresa = (empresaId: number, nomeEmpresa: string) => {
    mostrarMensagem(`Mensagem na tela`);
    if (!podeSelecionarEmpresa(empresaId)) {
      return;
    }
    definirEmpresa(empresaId, nomeEmpresa);
  };

  return (
    <section className="empresa-selecao">
      <h2>Seleção de empresa</h2>
      <p className="empresa-selecao-descricao">
        {subtitulo}
      </p>
      <div className="empresa-selecao-botoes">
        <Button
          type="button"
          onClick={() => selecionarEmpresa(1, "Alpha Ltda")}
        >
          Empresa Alpha
        </Button>
        <Button
          type="button"
          onClick={() => selecionarEmpresa(2, "Beta S.A.")}
        >
          Empresa Beta
        </Button>
        <Button type="button" onClick={limparEmpresa}>
          Limpar seleção
        </Button>
      </div>
    </section>
  );
}
