namespace Treinamento.Domain.Core.Notifications;

public class DomainNotificationHandler : IDomainNotificationHandler<DomainNotification>
{
    private List<DomainNotification> _notifications = [];

    public Task<IEnumerable<DomainNotification>> GetNotifications() =>
        Task.FromResult<IEnumerable<DomainNotification>>(_notifications);

    public Task Handle(DomainNotification message, CancellationToken cancellationToken = default)
    {
        _notifications.Add(message);
        return Task.CompletedTask;
    }

    public Task<bool> HasNotification() => Task.FromResult(_notifications.Count != 0);

    public void Dispose() => _notifications = [];
}
