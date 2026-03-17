using System;

namespace ITB.Domain.Core.Messages.Interfaces;

public interface IHandler<T> where T : ICommand 
{
    // Task Handle (T command);
    Task<CommandResult> Handle(T comando);
}
