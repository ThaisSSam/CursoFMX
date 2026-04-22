import { useUsuario } from "../hooks/useUsuario";

export const PaginaDashboard = () => {
  const { nomeUsuario } = useUsuario();
  return <h1>Bem-vindo, {nomeUsuario || "Visitante"}!</h1>;
};