using System;

namespace ITB.Domain.Core.Notifications;

/// <summary>
/// A implementação real. Esta classe "nasce" e "morre" a cada requisição HTTP.
/// </summary>
public class DomainNotificationHandler : IDomainNotificationHandler<DomainNotification>, IDisposable
{
    // A lista privada que guarda os post-its na memória
    private List<DomainNotification> _notifications;

    public DomainNotificationHandler()
    {
        // Sintaxe moderna do C# 12 (Collection Expression) para iniciar uma lista vazia
        _notifications = []; 
    }

    public Task<IEnumerable<DomainNotification>> GetNotifications()
    {
        return Task.FromResult(_notifications.AsEnumerable());
    }

    // A ação de "anotar" o erro na lista
    public Task Handle(DomainNotification message, CancellationToken cancellationToken = default)
    {
        _notifications.Add(message);
        return Task.CompletedTask; // Como a lista atua em memória RAM, finalizamos a Task instantaneamente
    }

    // Verifica se há pelo menos um erro na lista usando o poder do LINQ (.Any)
    public Task<bool> HasNotification()
    {
        return Task.FromResult(_notifications.Any());
    }

    // Limpa a memória para garantir que não haja vazamentos
    public void Dispose()
    {
        _notifications = [];
    }
}