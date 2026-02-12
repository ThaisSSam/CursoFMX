using System;

namespace ITB.Domain.Messages.Interfaces;

public class IMessageBus
{
    Task EnviarComando<T>(T comando) where T : ICommand;
}
