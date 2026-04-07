using System;
using ITB.Domain.Core.Messages;

namespace ITB.Domain.Core.Notifications;

/// <summary>
/// Este é o contrato do nosso Bloco de Notas. Ele define o que podemos fazer com as notificações.
/// </summary>
public interface IDomainNotificationHandler<T> where T : Message
{
    // Método usado para anotar um novo erro
    Task Handle(T message, CancellationToken cancellationToken = default);
    
    // Pergunta: Tem alguma anotação de erro aqui? (true/false)
    Task<bool> HasNotification();
    
    // Devolve a lista completa com todas as anotações feitas durante a requisição
    Task<IEnumerable<T>> GetNotifications();
}