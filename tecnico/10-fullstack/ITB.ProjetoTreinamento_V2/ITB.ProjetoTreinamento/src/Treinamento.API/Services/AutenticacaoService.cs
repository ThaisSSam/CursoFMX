using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.Data;
using Treinamento.Domain.Aggregates.Usuarios;

namespace Treinamento.Application.Services;

public class AutenticacaoService
{
    private readonly IUsuarioRepository _usuarioRepository; // Substitua pelo seu repositório ou DbContext

    public AutenticacaoService(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<(bool Sucesso, string MensagemErro)> LogarAsync(LoginRequest request)
    {
        // 1. Busca o usuário pelo e-mail
        var usuario = await _usuarioRepository.ObterPorEmailAsync(request.Email);

        // 2. Se o e-mail não existir, aplica a RN004 (Retorna erro genérico imediatamente)
        if (usuario == null)
        {
            return (false, "E-mail ou senha incorretos.");
        }

        // 3. Executa a lógica de autenticação dentro da Entidade de Domínio
        // Aqui simula a verificação da senha (se você usa criptografia, injete a validação aqui)
        bool loginValido = usuario.RealizarTentativaLogin(request.Password, (senhaDigitada, senhaDoBanco) => 
        {
            return senhaDigitada == senhaDoBanco; // Ajuste para a sua lógica de Hash de senha
        });

        // 4. Salva as mudanças no banco (persistindo o contador de erros ou zerando ele)
        await _usuarioRepository.AtualizarAsync(usuario);

        if (!loginValido)
        {
            // Pega o primeiro erro gerado na validação da sua Entity<T>
            return (false, usuario.ResultadoValidacao.Erros[0].MensagemErro);
        }

        // Tudo certo! (Aqui você geraria o token JWT se necessário)
        return (true, string.Empty);
    }
}