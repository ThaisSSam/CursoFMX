import { PaginaZustand } from "./pages/PaginaZustand";
import { PaginaContador } from "./pages/PaginaContador";
import { PaginaMostrar } from "./pages/PaginaMostrar.tsx";
import { UsuarioProvider } from "./contexts/UsuarioProvider";
import { PaginaPerfil } from "./pages/PaginaPerfil";
import { PaginaDashboard } from "./pages/PaginaDashboard";

export default function App() {
  return (
    <main className="app-shell">
      <UsuarioProvider>
        <section style={{ borderBottom: '1px solid #ccc', paddingBottom: '20px' }}>
          <h2>Exercício do dia 16/04:</h2>
          <PaginaPerfil />
          <PaginaDashboard />
        </section>
      </UsuarioProvider>

      <hr/>

      <PaginaZustand />
      <PaginaContador />
      <PaginaMostrar />
    </main>
  );
}