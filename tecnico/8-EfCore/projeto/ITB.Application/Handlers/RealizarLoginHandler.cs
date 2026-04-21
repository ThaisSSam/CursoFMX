using System;
using ITB.Application.Commands;
using ITB.Application.Dtos;
using ITB.Application.Interfaces;
using ITB.Domain.Core;
using ITB.Domain.Core.Exceptions;
using ITB.Domain.Core.Messages;
using ITB.Domain.Core.Messages.Interfaces;
using ITB.Domain.Interfaces;

namespace ITB.Application.Handlers;

// 1. C# 12: Injeção direto no nome da classe 
public class RealizarLoginHandler(
    IUsuarioReadRepository readRepository,
    ITokenService tokenService)
{
    // 2. O retorno agora é o Result Pattern 
    public async Task<Result<LoginResponseDto>> Handle(RealizarLoginCommand command)

    {
        // 3. Busca no banco SOMENTE pelo e-mail 
        var usuario = await readRepository.ObterPorEmailAsync(command.Email);

        // 4. Valida se existe e delega a validação da senha para a Entidade (BCrypt) 
        if (usuario == null || !usuario.ValidarSenha(command.Senha))
        {
            return Result<LoginResponseDto>.Failure("E-mail ou senha incorretos.");
        }

        // 5. A TRAVA DE SEGURANÇA! 
        // A senha está certa, mas é provisória? Devolvemos sucesso informando a trava. 
        if (usuario.PrecisaTrocarSenha)
        {
            return Result<LoginResponseDto>.Success(new LoginResponseDto
            {
                ExigeTrocaDeSenha = true
            });
        }

        // 6. Caminho Feliz: Gera o token e devolve com a trava desligada 
        var token = tokenService.GerarToken(usuario);

        return Result<LoginResponseDto>.Success(new LoginResponseDto
        {
            Token = token,
            ExigeTrocaDeSenha = false
        });
    }
}
