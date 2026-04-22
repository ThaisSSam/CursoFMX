import { useEffect, useState } from "react";
import axios from "axios";

type Usuario = {
  id: number;
  name: string;
  username: string;
  email: string;
};

export function UsuariosAxios() {
  const [usuarios, setUsuarios] = useState<Usuario[]>([]);
  const [carregando, setCarregando] = useState(false);
  const [erro, setErro] = useState("");

  async function carregar() {
    try {
      setCarregando(true);
      setErro("");

      const resposta = await axios.get<Usuario[]>(
        "https://jsonplaceholder.typicode.com/users",
      );
      setUsuarios(resposta.data);
    } catch {
      setErro("Não foi possível carregar usuários");
    } finally {
      setCarregando(false);
    }
  }

  useEffect(() => {
    carregar();
  }, []);

  if (carregando) return <p>Carregando...</p>;
  if (erro) return <p>{erro}</p>;

  return (
    <section className="page-card">
      <h2>Usuários com axios</h2>
      <ul>
        {usuarios.map((u) => (
          <div style={{borderBottom: "1px solid lightGrey", borderRight: "1px solid lightGrey", borderTop: "1px solid lightGrey", paddingTop:"1px", marginBottom:"20px"}}>
            <h5>Dados do usuário {u.id}</h5>
            <div style={{paddingLeft: "10px"}}>
              <p>User: {u.username}</p>
              <p>Nome: {u.name}</p>
              <p>Email: {u.email}</p>
            </div>
          </div>
          // <li key={u.id}>
          //   {u.name} - {u.email}
          // </li>
        ))}
      </ul>
    </section>
  );
}
