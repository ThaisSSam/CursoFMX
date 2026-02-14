using System;

namespace ITB.Domain.Core.Messages.Interfaces;

public interface IMessageBus 
{
    Task EnviarComando<T>(T comando) where T : ICommand;
}