using System;
using ITB.Domain.Core.Notifications;
using Microsoft.AspNetCore.Mvc;

namespace ITB.API.Controller.Base;

public class BaseController : ControllerBase
{
  protected readonly IDomainNotificationHandler<DomainNotification> _notifications;

  protected BaseController(IDomainNotificationHandler<DomainNotification> notifications)
  {
    _notifications = notifications;
  }

  protected async Task<bool> OperacaoValida()=>!await _notifications.HasNotification();

  protected new async Task<ActionResult> Response(object? result = null)
  {
    if(await OperacaoValida())
    {
      return Ok (new ApiResponse<object>(result!, "Operação realizada"));
    }

    var notificacoes = await _notifications.GetNotifications();

    var mensagensDeErro = notificacoes.Select(nameof=>nameof.Value).ToList();

    return BadRequest(new ApiResponse<object>("Erros de validação encontrados", mensagensDeErro));
  }
}
