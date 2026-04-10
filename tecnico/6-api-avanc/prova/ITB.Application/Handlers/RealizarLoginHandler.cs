using ITB.Application.Commands;
using ITB.Application.Interfaces;
using ITB.Domain.Core.Messages.Interfaces;
using ITB.Domain.Core.Notifications;
using ITB.Domain.Interfaces;

namespace ITB.Application.Handlers;

public class RealizarLoginHandler : IHandler<RealizarLoginCommand>
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly ITokenService _tokenService;
    private readonly IDomainNotificationHandler<DomainNotification> _notifications;

    public RealizarLoginHandler(
        IUsuarioRepository usuarioRepository,
        ITokenService tokenService,
        IDomainNotificationHandler<DomainNotification> notifications)
    {
        _usuarioRepository = usuarioRepository;
        _tokenService = tokenService;
        _notifications = notifications;
    }

    public async Task Handle(RealizarLoginCommand comando)
    {
        var usuario = await _usuarioRepository.ObterPorEmailESenhaAsync(comando.Email, comando.Senha);

        if (usuario == null)
        {
            await _notifications.Handle(new DomainNotification("Auth", "Usuário ou senha inválidos."));
            return;
        }

        var token = _tokenService.GerarToken(usuario);

        // Se por algum motivo o token vier nulo, evitamos o erro 500 aqui
        if (string.IsNullOrEmpty(token))
        {
            await _notifications.Handle(new DomainNotification("Erro", "Falha ao gerar o token de acesso."));
            return;
        }

        await _notifications.Handle(new DomainNotification("AuthToken", token));
    }
}