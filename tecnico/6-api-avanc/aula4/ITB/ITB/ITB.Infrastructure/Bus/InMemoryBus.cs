using System;
using ITB.Domain.Core.Messages;
using Microsoft.Extensions.DependencyInjection;

namespace ITB.Infrastructure.Bus;

public sealed class InMemoryBus : IMessageBus
{
    private readonly IServiceProvider _serviceProvider;
    public InMemoryBus(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

    public async Task EnviarComando<T>(T comando) where T : ICommand
    {
        var handlers = _serviceProvider.GetServices<IHandler<T>>();

        foreach (var handler in handlers)
        await handler.Handle(comando);
    }
}
