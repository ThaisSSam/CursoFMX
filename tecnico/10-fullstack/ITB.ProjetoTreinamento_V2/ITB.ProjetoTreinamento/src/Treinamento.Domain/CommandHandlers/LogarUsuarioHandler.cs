using System;
using System.Threading.Tasks;
using Treinamento.Domain.Aggregates.Usuarios;
using Treinamento.Domain.Core.Interfaces;

namespace Treinamento.Domain.Handlers;

public class LogarUsuarioHandler : ILogarUsuarioHandler
{
    private readonly IUsuarioRepository _usuarioRepository;

    public LogarUsuarioHandler(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<(bool Sucesso, string MensagemErro)> ExecutarAsync(LoginDto request)
    {
        var usuario = await _usuarioRepository.ObterPorEmailAsync(request.Email);

        if (usuario == null)
        {
            return (false, "E-mail ou senha incorretos. Verifique suas credenciais e tente novamente.");
        }

        bool loginValido = usuario.RealizarTentativaLogin(request.Senha, (senhaDigitada, senhaDoBanco) => 
        {
            return senhaDigitada == senhaDoBanco; 
        });

        // Salva o estado atualizado
        await _usuarioRepository.LogarAsync(usuario);

        if (!loginValido)
        {
            return (false, usuario.ResultadoValidacao.Erros[0].MensagemErro);
        }

        return (true, string.Empty);
    }
}