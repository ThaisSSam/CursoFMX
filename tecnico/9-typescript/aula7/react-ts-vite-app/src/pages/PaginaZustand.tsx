import Button from "../components/Button";
import { useMensagemZustandStore } from "../store/useMensagemZustandStore";

export function PaginaZustand() {
  const { mensagem, definirMensagem, limparMensagem } = useMensagemZustandStore();

  return (
    <section className="page-card">
      <h2>Pagina Zustand</h2>
      <p className="descricao">
        Esta pagina usa Zustand para compartilhar estado sem Provider.
      </p>
      <p>
        <strong>Mensagem atual:</strong> {mensagem || "Nenhuma"}
      </p>
      <div className="actions">
        <Button
          type="button"
          onClick={() => definirMensagem("Mensagem definida pela pagina Zustand")}
        >
          Definir mensagem
        </Button>
        <Button type="button" onClick={limparMensagem}>
          Limpar
        </Button>
      </div>
    </section>
  );
}
