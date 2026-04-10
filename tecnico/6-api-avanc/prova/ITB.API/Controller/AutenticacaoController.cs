using ITB.Application.Commands;
using ITB.Application.Handlers;
using ITB.Domain.Core.Notifications;
using ITB.API.Controller.Base;
using ITB.Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITB.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AutenticacaoController : BaseController
{
    private readonly RealizarLoginHandler _handler;
    // Removida a declaração duplicada de _notifications que causava o erro!

    public AutenticacaoController(
        RealizarLoginHandler handler,
        IDomainNotificationHandler<DomainNotification> notifications) 
        : base(notifications)
    {
        _handler = handler;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var comando = new RealizarLoginCommand { Email = request.Email, Senha = request.Senha };
        
        await _handler.Handle(comando);

        // ATENÇÃO: Use a variável que vem da BaseController (protected)
        var notifications = await _notifications.GetNotifications();
        var tokenNode = notifications.FirstOrDefault(n => n.Key == "AuthToken");

        return await Response(tokenNode != null ? new { token = tokenNode.Value } : null);
    }
}