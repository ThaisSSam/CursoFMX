import api from './config';
import { loginEndpoints } from './endpoints/login';

export async function fazerLoginSimples(email: string, senha: string) {
  const corpoRequest = {
    Email: email.trim(),
    Senha: senha,
    LembrarAcesso: true
  };

  try {
    const { data } = await api.post('/usuarios/login', corpoRequest);

    const tokenGerado = data?.token || data?.data?.token;

    if (tokenGerado) {
      localStorage.setItem('auth_token', tokenGerado);
      localStorage.setItem('token', tokenGerado);
    }

    return { sucesso: true, token: tokenGerado };
  } catch (erro: any) {
    const respostaErro = erro.response?.data;

    const mensagemErro =
      respostaErro?.errors?.[0] ||
      erro.message ||
      'Erro ao tentar realizar o login.';

    return { sucesso: false, erro: mensagemErro };
  }
}

export async function esqueciSenhaService(email: string) {
  try {
    return await loginEndpoints.solicitarRecuperacao(email);
  } catch (erro: any) {
    throw new Error(erro.message);
  }
}

// Função de logout 
export async function sairDoSistema() {
  await loginEndpoints.executarLogout();

  localStorage.removeItem('auth_token');
  localStorage.removeItem('token');
  localStorage.removeItem('refresh_token');
  localStorage.removeItem('auth_user');
}

export function tokenEstaExpirado(): boolean {
  const expiraEm = localStorage.getItem('auth_expires_at');
  if (!expiraEm) return false;
  return new Date(expiraEm) <= new Date();
}