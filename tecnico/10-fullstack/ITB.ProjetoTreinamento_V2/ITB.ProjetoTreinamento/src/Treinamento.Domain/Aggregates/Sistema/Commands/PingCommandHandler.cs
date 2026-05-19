using Treinamento.Domain.Core.Events;
using Treinamento.Domain.Core.Interfaces;
using Treinamento.Domain.Core.Notifications;
using Treinamento.Domain.CommandHandlers;
using Treinamento.Domain.Core.Bus;

namespace Treinamento.Domain.Aggregates.Sistema.Commands;

public class PingCommandHandler(
    IUnitOfWork uow,
    IBus bus,
    IDomainNotificationHandler<DomainNotification> notifications)
    : CommandHandler(uow, bus, notifications), IHandler<PingCommand>
{
    public Task Handle(PingCommand message, CancellationToken cancellationToken = default)
    {
        message.Resposta = $"pong: {message.Mensagem}";
        return Task.CompletedTask;
    }
}
