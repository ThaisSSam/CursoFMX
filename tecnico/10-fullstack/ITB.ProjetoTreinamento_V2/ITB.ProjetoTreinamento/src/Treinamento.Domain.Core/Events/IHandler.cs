namespace Treinamento.Domain.Core.Events;

public interface IHandler<in T> where T : Message
{
    Task Handle(T message, CancellationToken cancellationToken = default);
}
