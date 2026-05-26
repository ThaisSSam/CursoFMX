import api from './config'; 

export async function fazerLoginSimples(email, senha) {
  const corpoRequest = {
    Email: email.trim(),
    Senha: senha,
    LembrarAcesso: true 
  };

  try {
    const { data } = await api.post('/usuarios/login', corpoRequest);
    
    localStorage.setItem('auth_token', data.token);
    
    return { sucesso: true, token: data.token };

  } catch (erro) {
    const respostaErro = erro.response?.data;
    const mensagemErro = 
      respostaErro?.errors?.[0] || 
      erro.message || 
      'Erro ao tentar realizar o login.';


    return { sucesso: false, erro: mensagemErro };
  }
}