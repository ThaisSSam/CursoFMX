// import { MensagemProvider } from "./contexts/MensagemProvider";
// import { PaginaContexto } from "./pages/PaginaContexto";
import { PaginaZustand } from "./pages/PaginaZustand";
import {PaginaContador} from "./pages/PaginaContador";
import {PaginaMostrar} from "./pages/PaginaMostrar.tsx"


// function PaginaComContexto() {
//   return (
//     <MensagemProvider>
//       <PaginaContexto />
//     </MensagemProvider>
//   );
// }

export default function App() {

  return (
    <main className="app-shell">
      {/* <PaginaComContexto /> */}
      <PaginaZustand />
      <PaginaContador />
      <PaginaMostrar />
    </main>
  );
}
