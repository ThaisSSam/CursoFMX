using Treinamento.Domain.Core.Events;

namespace Treinamento.Domain.Core.Notifications;

public interface IDomainNotificationHandler<in T> : IHandler<T> where T : Message
{
    Task<IEnumerable<DomainNotification>> GetNotifications();
    Task<bool> HasNotification();
}
