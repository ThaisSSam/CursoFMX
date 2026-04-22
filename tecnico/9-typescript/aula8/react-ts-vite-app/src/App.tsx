import { Link, Route, Routes } from "react-router-dom";
import { Home } from "./pages/Home";
import { UsuariosFetch } from "./pages/UsuariosFetch";
import { UsuariosAxios } from "./pages/UsuariosAxios";
import { Navegacao } from "./pages/Navegacao";
import { Exercicio } from "./pages/Exercicio";

export default function App() {
  return (
    <main className="app-shell">
      <h2 
      style={{
        marginBottom: "1px"
      }}
      >
        Navegação e consumo de API
      </h2>
      <nav className="menu">
        <Link to="/">Home</Link>
        <Link to="/navegacao">Navegação</Link>
        <Link to="/usuarios-fetch">Usuarios (fetch)</Link>
        <Link to="/usuarios-axios">Usuarios (axios)</Link>
        <Link to="/exercicio">Exercício</Link>
      </nav>

      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/usuarios-fetch" element={<UsuariosFetch />} />
        <Route path="/usuarios-axios" element={<UsuariosAxios />} />
        <Route path="/navegacao" element={<Navegacao />} />
        <Route path="/exercicio" element={<Exercicio />} />
      </Routes>
    </main>
  );
}
