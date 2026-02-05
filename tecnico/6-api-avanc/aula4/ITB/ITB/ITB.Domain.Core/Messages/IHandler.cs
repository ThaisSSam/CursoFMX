using System;

namespace ITB.Domain.Core.Messages;

public interface IHandler<T> where T : ICommand
{
    Task Handle(T comando);
}
