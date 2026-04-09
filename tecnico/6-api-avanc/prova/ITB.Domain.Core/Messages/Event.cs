using System;

namespace ITB.Domain.Core.Messages.Interfaces;

public abstract class Event : Message 
{ 
    // Registra o momento exato do evento 
    public DateTime Timestamp { get; private set; } 

    protected Event() => Timestamp = DateTime.UtcNow; 
}