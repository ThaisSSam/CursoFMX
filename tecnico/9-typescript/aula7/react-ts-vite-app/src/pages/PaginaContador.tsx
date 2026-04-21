import { useMensagemZustandStore } from "../store/useMensagemZustandStore"

export const PaginaContador=()=>{
  const{contador, incrementarContador, decrementarContador} = useMensagemZustandStore();
  return (
    <div>
      <h1>Página Contador</h1>
      <p>{contador}</p>
      <button onClick={()=> incrementarContador()}>Incremetar</button>
      <button onClick={()=> decrementarContador()}>Decremetar</button>
    </div>
  )
}