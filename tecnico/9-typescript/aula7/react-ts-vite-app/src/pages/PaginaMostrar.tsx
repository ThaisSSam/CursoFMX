import Button from "../components/Button";
import {useMostrar} from "../store/useMostrar";

export function PaginaMostrar() {
  const{mensagem, mostrar}= useMostrar();
  return(
    <section>
      <p>{mensagem}</p>
      <Button onClick={mostrar}>Mostrar</Button>
    </section>
  )
}