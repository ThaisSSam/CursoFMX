using System;
using ITB.Domain.Messages.Interfaces;

namespace ITB.Infrastructure.Bus;

public class InMemoryBus : IMessageBus
{
    private readonly IServiceProvider _serviceProvider;
    public InMemoryBus(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    public async Task EnviarComando<T>(T comando) where T : ICommand
    {
        // Busca quem sabe resolver esse comando específico
        var handlers = _serviceProvider.GetServices<IHandler<T>>();
        foreach (var handler in handlers)
        {
            await handler.Handle(comando);
        }
    }
}
