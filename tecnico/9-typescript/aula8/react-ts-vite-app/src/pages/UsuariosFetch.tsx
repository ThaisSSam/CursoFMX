import { useEffect, useState } from "react";

type Usuario = {
  id: number;
  name: string;
  email: string;
};

export function UsuariosFetch() {
  const [usuarios, setUsuarios] = useState<Usuario[]>([]);
  const [carregando, setCarregando] = useState(false);
  const [erro, setErro] = useState("");

  // async function carregar() { js vanilla
  const carregar = async () => {
    try {
      setCarregando(true);
      setErro("");

      const resposta = await fetch("https://jsonplaceholder.typicode.com/users");
      if (!resposta.ok) {
        throw new Error("Falha ao buscar usuários");
      }
      const dados = (await resposta.json()) as Usuario[];
      setUsuarios(dados);

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
      <h2>Usuarios com fetch</h2>
      <ul>
        {usuarios.map((u) => (
          <div style={{borderBottom: "1px solid lightGrey", borderRight: "1px solid lightGrey", borderLeft: "1px solid lightGrey", paddingTop:"1px", marginBottom:"20px"}}>
            <p style={{marginLeft:"10px"}}>Id: {u.id}</p>
            <p style={{marginLeft:"10px"}}>Nome: {u.name} </p>
            <p style={{marginLeft:"10px"}}>Email: {u.email}</p>
          </div>
        ))}
      </ul>
    </section>
  );
}
