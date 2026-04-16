import Button from "../components/Button";
import { useMensagem } from "../hooks/useMensagem";

export function PaginaContexto() {
  const { mensagem, definirMensagem, limparMensagem } = useMensagem();

  return (
    <section className="page-card">
      <h2>Pagina Context/Provider</h2>
      <p className="descricao">
        Esta pagina usa React Context para compartilhar estado com Provider.
      </p>
      <p>
        <strong>Mensagem atual:</strong> {mensagem || "Nenhuma"}
      </p>
      {/* <div>
        <label htmlFor="mostrar" id="mostrar">Mostrar</label>
        <input type="checkbox" aria-label="mostrar"/>
      </div> */}
      <div className="actions">
        <Button
          type="button"
          onClick={() => definirMensagem("Mensagem definida pela pagina Context")}
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
