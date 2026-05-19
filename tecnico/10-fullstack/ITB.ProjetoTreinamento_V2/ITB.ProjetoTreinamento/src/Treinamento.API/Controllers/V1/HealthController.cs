using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Treinamento.API.Controllers.Base;
using Treinamento.Domain.Aggregates.Sistema.Commands;
using Treinamento.Domain.Core.Bus;
using Treinamento.Domain.Core.Notifications;

namespace Treinamento.API.Controllers.V1;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/health")]
public class HealthController(
    IDomainNotificationHandler<DomainNotification> notifications,
    IBus bus) : BaseController(notifications)
{
    private readonly IBus _bus = bus;

  [HttpGet("ping")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Ping(CancellationToken cancellationToken)
    {
        var command = new PingCommand { Mensagem = "health" };
        await _bus.SenderCommand(command, cancellationToken);
        return await Responder(new
        {
            status = "ok",
            mensagem = command.Resposta,
            dataHoraUtc = DateTime.UtcNow
        });
    }
}
