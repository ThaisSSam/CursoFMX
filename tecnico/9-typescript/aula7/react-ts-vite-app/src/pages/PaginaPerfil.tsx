import { useUsuario } from "../hooks/useUsuario";

export const PaginaPerfil = () => {
  const { nomeUsuario, definirNome, limparNome } = useUsuario();
  return (
    <div>
      <input
        type="text"
        onChange={(e) => definirNome(e.target.value)}
        value={nomeUsuario}
        placeholder="Nome"
      />
      <button 
        onClick={limparNome}
      >
        Limpar
      </button>
    </div>
  );
};