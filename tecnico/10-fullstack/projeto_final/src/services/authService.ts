import type { AxiosError } from 'axios';
import api from './config';
export interface LoginRequestData {
  Email: string;
  Senha: string;
  LembrarAcesso: boolean;
}

export interface LoginResponseSuccess {
  message?: string;
  mensagem?: string;
  token?: string;
  accessToken?: string;
  AccessToken?: string;
}

export interface RespostaApi<T> {
  data?: T;
  dados?: T;
  success: boolean;
  message?: string;
  mensagem?: string;
  errors?: string[] | null;
}

export interface ResultadoLogin {
  tokenAcesso: string | null;
  message: string;
}

function extrairCargaLogin<T>(data: RespostaApi<T> | T): T {
  const envelope = data as RespostaApi<T>;
  return (envelope?.data ?? envelope?.dados ?? data) as T;
}

function montarResultadoLogin(
  carga: any,
  dataRaw?: RespostaApi<any>
): ResultadoLogin {
  const token = carga?.token ?? carga?.accessToken ?? carga?.AccessToken;
  const mensagem = dataRaw?.message ?? dataRaw?.mensagem ?? carga?.message ?? carga?.mensagem ?? "Operação realizada com sucesso.";

  if (!token) {
    throw new Error(dataRaw?.message ?? dataRaw?.mensagem ?? 'A autenticação falhou.');
  }

  return {
    tokenAcesso: token,
    message: mensagem,
  };
}

export async function login(credenciais: LoginRequestData): Promise<ResultadoLogin> {
  try {
    const { data } = await api.post<RespostaApi<any>>('/api/v1/auth/login', {
      email: credenciais.Email,
      password: credenciais.Senha,
    });

    const cargaExtraida = extrairCargaLogin(data);
    const resultado = montarResultadoLogin(cargaExtraida, data);

    if (resultado.tokenAcesso) {
      localStorage.setItem('auth_token', resultado.tokenAcesso);
    }

    return resultado;
  } catch (error: unknown) {
    const axiosErro = error as AxiosError<RespostaApi<any>>;
    const mensagemTratada = axiosErro.response?.data?.errors?.[0] || axiosErro.response?.data?.message || axiosErro.message || "Erro desconhecido ao efetuar login.";
    throw new Error(mensagemTratada);
  }
}

export async function recuperarSenha(email: string): Promise<{ success: boolean; message: string }> {
  try {
    const { data } = await api.post<RespostaApi<unknown>>(
      '/api/v1/auth/esqueci-senha',
      { email: String(email ?? '').trim() }
    );
    
    return {
      success: data?.success ?? true,
      message: data?.message ?? data?.mensagem ?? 'Instruções de recuperação enviadas com sucesso.',
    };
  } catch (error: unknown) {
    const axiosErro = error as AxiosError<RespostaApi<any>>;
    const mensagemTratada = axiosErro.response?.data?.errors?.[0] || axiosErro.message || "Erro ao solicitar recuperação de senha.";
    throw new Error(mensagemTratada);
  }
}