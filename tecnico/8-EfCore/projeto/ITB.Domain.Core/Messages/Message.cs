using System;

namespace ITB.Domain.Core.Messages;

public abstract class Message
{
    // Define o tipo da mensagem (ex: "DomainNotification")
    public string MessageType { get; protected set; }
    
    // Identificador do agregado (grupo de dados) relacionado
    public Guid AggregateId { get; protected set; }

    protected Message() => MessageType = GetType().Name;
}