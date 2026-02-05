using System;

namespace ITB.Domain.Core.Messages;

public interface IMessageBus
{
    Task EnviarComando<T>(T comando) where T : ICommand;
}
