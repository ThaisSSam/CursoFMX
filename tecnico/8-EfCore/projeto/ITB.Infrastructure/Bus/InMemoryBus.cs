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

    public async Task EnviarComando<T>(T comando) where T : ICommand
    {
        var handlers = _serviceProvider.GetServices<IHandler<T>>();

        // CommandResult? resultadoFinal = null;

        
        foreach (var handler in handlers)
        {
            await handler.Handle(comando);
            // var resultadoAtual =  await handler.Handle(comando);
            // if (resultadoAtual != null && !resultadoAtual.Sucesso)
            // {
            //     return resultadoAtual;
            // }

            // if (resultadoFinal == null | (resultadoAtual != null && resultadoAtual.Dados != null))
            // {
            //     resultadoFinal = resultadoAtual;
            // } 
        }

        //return resultadoFinal ?? new CommandResult(false, "Nenhum manipulador (handler) encontrado para este comando.");
    }
}
