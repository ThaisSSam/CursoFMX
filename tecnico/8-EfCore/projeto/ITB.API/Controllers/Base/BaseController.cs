using System;
using ITB.Domain.Core.Messages;
using Microsoft.AspNetCore.Mvc;

namespace ITB.API.Controllers.Base;

[ApiController]
public class BaseController : ControllerBase
{
    protected readonly IDomainNotificationHandler<DomainNotification> _notifications;

    public BaseController(IDomainNotificationHandler<DomainNotification> notifications)
    {
        _notifications = notifications;
    }

    protected async Task<bool> OperacaoValida() => !await _notifications.HasNotification();

    protected new async Task<ActionResult> Response(object? result = null)
    {
        if (await OperacaoValida())
        {
            return Ok(new ApiResponse<object>(result!, "Operação realizada com sucesso."));
        }

        var notificacoes = await _notifications.GetNotifications();

        var mensagensDeErro = notificacoes.Select(n => n.Value).ToList();

        return BadRequest(new ApiResponse<object>("Erros de validação encontrados", mensagensDeErro));
    }
}
