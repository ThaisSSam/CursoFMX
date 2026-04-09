using System;
using ITB.Domain.Core.Messages;
using ITB.Domain.Core.Messages.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ITB.Infrastructure.Bus;

public class InMemoryBus : IMessageBus
{
    private readonly IServiceProvider _serviceProvider;
    public InMemoryBus(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    // public async Task EnviarComando<T>(T comando) where T : ICommand
    // {   
    //     var handlers = _serviceProvider.GetServices<IHandler<T>>();
    //     foreach (var handler in handlers)
    //     {
    //         await handler.Handle(comando);
    //     }
    // }
    public async Task<CommandResult> EnviarComando<T>(T comando) where T : ICommand
    {   
        var handlers = _serviceProvider.GetServices<IHandler<T>>();

        if (handlers == null || !handlers.Any())
        {
            return new CommandResult(false, "Nenhum manipulador encontrado.");
        }

        foreach (var handler in handlers)
        {    
            await handler.Handle(comando);
        }

        return new CommandResult(true, "Comando processado com sucesso!", null);
    }
}
