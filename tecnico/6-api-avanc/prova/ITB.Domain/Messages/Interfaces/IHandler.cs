using System;

namespace ITB.Domain.Messages.Interfaces;

public class IHandler<T> where T : ICommand 
{
    Task Handle (T command);
}
