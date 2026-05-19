using Treinamento.Domain.Core.Events;

namespace Treinamento.Domain.Core.Notifications;

public class DomainNotification(string key, string value) : Event
{
    public string Key { get; private set; } = key;
    public string Value { get; private set; } = value;
}
