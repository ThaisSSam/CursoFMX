using System.Threading.Tasks; // OBRIGATÓRIO
using ITB.Application.Commands;
using ITB.Application.Interfaces;
using ITB.Domain.Core.Messages.Interfaces;
using ITB.Domain.Interfaces;

namespace ITB.Application.Handlers;

public class RealizarLoginHandler : IHandler<RealizarLoginCommand>
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly ITokenService _tokenService;

    public RealizarLoginHandler(IUsuarioRepository usuarioRepository, ITokenService tokenService)
    {
        _usuarioRepository = usuarioRepository;
        _tokenService = tokenService;
    }

    // A assinatura deve ser EXATAMENTE igual à interface: Task e não Task<CommandResult>
    public async Task Handle(RealizarLoginCommand request) 
    {
        var usuario = await _usuarioRepository.ObterPorEmailESenhaAsync(request.Email, request.Senha);

        // Se não achar o usuário, apenas para. O erro será tratado via Notificações ou pela Controller
        if (usuario == null) return;

        // Gera o token e guarda no comando para a Controller ler depois
        var token = _tokenService.GerarToken(usuario);
        request.TokenGerado = token; 
    }
}