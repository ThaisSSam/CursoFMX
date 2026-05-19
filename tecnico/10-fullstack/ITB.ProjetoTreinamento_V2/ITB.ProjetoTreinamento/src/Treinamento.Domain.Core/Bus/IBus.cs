using Treinamento.Domain.Core.Commands;
using Treinamento.Domain.Core.Events;
using Treinamento.Domain.Core.Queries;

namespace Treinamento.Domain.Core.Bus;

public interface IBus
{
    Task SenderCommand<T>(T command, CancellationToken cancellationToken = default) where T : Command;

    Task RaiseEvent<T>(T event_, CancellationToken cancellationToken = default) where T : Event;

    Task<TResult> SendQuery<TQuery, TResult>(TQuery query, CancellationToken cancellationToken = default)
        where TQuery : IQuery<TResult>;
}
