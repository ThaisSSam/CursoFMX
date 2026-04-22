import { useContext } from "react";
import { UsuarioContext } from "../contexts/usuarioContext";

export const useUsuario = () => {
  const context = useContext(UsuarioContext);
  if (!context) throw new Error("useUsuario deve ser usado dentro de um UsuarioProvider");
  return context;
};