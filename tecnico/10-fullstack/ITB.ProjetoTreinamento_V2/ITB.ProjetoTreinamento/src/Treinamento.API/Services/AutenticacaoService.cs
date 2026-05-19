using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.Data;
using Treinamento.Domain.Aggregates.Usuarios;

namespace Treinamento.Application.Services;

public class AutenticacaoService
{
    private readonly IUsuarioRepository _usuarioRepository;

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
        bool loginValido = usuario.RealizarTentativaLogin(request.Password, (senhaDigitada, senhaDoBanco) => 
        {
            return senhaDigitada == senhaDoBanco; 
        });

        // 4. Salva as mudanças no banco (persistindo o contador de erros ou zerando ele)
        await _usuarioRepository.AtualizarAsync(usuario);

        // 5. Se o login falhou, extrai a MensagemErro do primeiro item da lista da sua Entity
        if (!loginValido)
        {
            return (false, usuario.ResultadoValidacao.Erros[0].MensagemErro);
        }

        // Tudo certo! (Pronto para gerar o token JWT ou cookies de sessão)
        return (true, string.Empty);
    }
}