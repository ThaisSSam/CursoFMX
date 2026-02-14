using System;
using ITB.Domain.Core.Messages;
using Microsoft.Extensions.Logging;
using ITB.Domain.Core.Messages.Interfaces;

namespace ITB.Application.Handlers;

public class LogComandoGenericoHandler<T> : IHandler<T> where T : ICommand
{
    private readonly ILogger<LogComandoGenericoHandler<T>> _logger;

    public LogComandoGenericoHandler(ILogger<LogComandoGenericoHandler<T>> logger)
    {
        _logger = logger;
    }

    public async Task Handle(T comando)
    {
        _logger.LogInformation("Executando Comando: {CommandName} | Dados Enviados: {@Data}",
            typeof(T).Name, 
            comando);
        await Task.CompletedTask;
    }
    
}
