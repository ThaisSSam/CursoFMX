using ITB.Domain.Core.Messages;
using ITB.Domain.Core.Messages.Interfaces;
using Microsoft.Extensions.Logging;

namespace ITB.Application.Handlers;

public class LogComandoGenericoHandler<T> : IHandler<T> where T : ICommand
{
    private readonly ILogger<LogComandoGenericoHandler<T>> _logger;
    public LogComandoGenericoHandler(ILogger<LogComandoGenericoHandler<T>> logger)
    {
        _logger = logger;
    }

    public Task Handle(T comando)
    {
        _logger.LogInformation("Injetando Comando: {CommandName} | Dados Enviados: {@Data}",
            typeof(T).Name,
            comando);

        return Task.FromResult(new CommandResult(
            sucesso: true,
            mensagem: ""
        ));
    }
}
