using ITB.Application.Commands;
using ITB.Application.Interfaces;
using ITB.Domain.Core.Messages.Interfaces; 
using ITB.Domain.Core.Commands; 
using ITB.Domain.Interfaces;
using ITB.Domain.Entities;
using ITB.Domain.Core.Messages;

namespace ITB.Application.Handlers;

public class RealizarLoginHandler : IHandler<RealizarLoginCommand>
{
    private readonly IUsuarioRepository _usuarioRepository; // Trocou o contexto pelo repositório
    private readonly ITokenService _tokenService;

    public RealizarLoginHandler(IUsuarioRepository usuarioRepository, ITokenService tokenService)
    {
        _usuarioRepository = usuarioRepository;
        _tokenService = tokenService;
    }

    public async Task<CommandResult> Handle(RealizarLoginCommand request)
    {
        var usuario = await _usuarioRepository.ObterPorEmailESenha(request.Email, request.Senha);

        if (usuario == null)
            return new CommandResult(false, "Usuário ou senha incorretos.", null);

        var token = _tokenService.GerarToken(usuario);
        return new CommandResult(true, "Login realizado com sucesso!", token);
    }

    Task<CommandResult> IHandler<RealizarLoginCommand>.Handle(RealizarLoginCommand comando)
    {
        throw new NotImplementedException();
    }
}