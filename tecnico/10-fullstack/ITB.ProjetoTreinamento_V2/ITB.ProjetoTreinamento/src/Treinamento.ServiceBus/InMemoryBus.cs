using Treinamento.Domain.Core.Bus;
using Treinamento.Domain.Core.Commands;
using Treinamento.Domain.Core.Events;
using Treinamento.Domain.Core.Notifications;
using Treinamento.Domain.Core.Queries;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Treinamento.ServiceBus;

public sealed class InMemoryBus(
    IBusRequestServicesResolver serviceScopeResolver,
    ILogger<InMemoryBus> logger) : IBus
{
    public Task RaiseEvent<T>(T eventData, CancellationToken cancellationToken = default) where T : Event =>
        Publish(eventData, cancellationToken);

    public Task SenderCommand<T>(T command, CancellationToken cancellationToken = default) where T : Command
    {
        logger.LogInformation("Despachando comando no bus. CommandType={CommandType}", typeof(T).Name);
        return Publish(command, cancellationToken);
    }

    public async Task<TResult> SendQuery<TQuery, TResult>(TQuery query, CancellationToken cancellationToken = default)
        where TQuery : IQuery<TResult>
    {
        await using var busScope = serviceScopeResolver.CriarEscopoParaDespacho();
        var container = busScope.Services;

        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(typeof(TQuery), typeof(TResult));
        var handler = container.GetService(handlerType);

        if (handler is null)
        {
            logger.LogWarning("Nenhum handler registrado para query. QueryType={QueryType}", typeof(TQuery).Name);
            throw new InvalidOperationException(
                $"Nenhum handler registrado para a query '{typeof(TQuery).Name}'. " +
                "Registre IQueryHandler<TQuery, TResult> no container de DI.");
        }

        if (handler is IQueryHandler<TQuery, TResult> queryHandler)
            return await queryHandler.Handle(query, cancellationToken);

        throw new InvalidOperationException(
            $"O handler registrado para '{typeof(TQuery).Name}' não implementa IQueryHandler<{typeof(TQuery).Name}, {typeof(TResult).Name}>.");
    }

    private async Task Publish<T>(T message, CancellationToken cancellationToken = default) where T : Message
    {
        await using var busScope = serviceScopeResolver.CriarEscopoParaDespacho();
        var container = busScope.Services;

        var handlerType = message is DomainNotification
            ? typeof(IDomainNotificationHandler<T>)
            : typeof(IHandler<T>);

        var handler = container.GetService(handlerType);

        if (handler is null)
        {
            if (message is Command)
            {
                logger.LogError("Nenhum handler registrado para comando. CommandType={CommandType}", typeof(T).Name);
                throw new InvalidOperationException(
                    $"Nenhum handler registrado para o comando '{typeof(T).Name}'. Registre IHandler<{typeof(T).Name}> no container de DI.");
            }

            logger.LogDebug("Mensagem sem handler registrado. MessageType={MessageType}", typeof(T).Name);
            return;
        }

        if (handler is IHandler<T> typedHandler)
            await typedHandler.Handle(message, cancellationToken);
    }
}
