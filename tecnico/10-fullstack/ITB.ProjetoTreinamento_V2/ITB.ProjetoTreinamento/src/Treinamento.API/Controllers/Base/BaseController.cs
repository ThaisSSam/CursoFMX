using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Treinamento.Domain.Core.Notifications;

namespace Treinamento.API.Controllers.Base;

[ApiController]
[Produces("application/json")]
public abstract class BaseController(IDomainNotificationHandler<DomainNotification> notifications) : ControllerBase
{
    private readonly IDomainNotificationHandler<DomainNotification> _notifications = notifications;

    protected async Task<IReadOnlyCollection<string>> ObterErros()
    {
        var notifications = (IReadOnlyCollection<DomainNotification>)await _notifications.GetNotifications();
        return [.. notifications.Select(n => n.Value)];
    }

    protected async Task NotifyErrorModelInvalid(ModelStateDictionary modelState)
    {
        foreach (var error in modelState.Values.SelectMany(e => e.Errors))
        {
            var errorMsg = error.Exception?.Message ?? error.ErrorMessage;
            await _notifications.Handle(new DomainNotification(string.Empty, errorMsg));
        }
    }

    protected async Task<bool> ModelStateIsValid()
    {
        if (ModelState.IsValid) return true;
        await NotifyErrorModelInvalid(ModelState);
        return false;
    }

    protected async Task<bool> OperationValid() => !await _notifications.HasNotification();

    protected async Task<ActionResult> Responder(object? result = null)
    {
        if (await OperationValid())
        {
            return Ok(new { success = true, data = result });
        }

        var erros = await ObterErros();
        return BadRequest(new ApiResponse<object>("Erro de validação", [.. erros]));
    }
}
