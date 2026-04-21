using ITB.Application.Handlers;
using ITB.Domain.Core.Messages;
using ITB.Domain.Core.Messages.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ITB.Infrastructure.Bus;

public sealed class InMemoryBus : IMessageBus
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IDomainNotificationHandler<DomainNotification> _notifications;

    public InMemoryBus(IServiceProvider serviceProvider,
        IDomainNotificationHandler<DomainNotification> notifications)
    {
        _serviceProvider = serviceProvider;
        _notifications = notifications;
    }

    public async Task EnviarComando<T>(T comando) where T : ICommand
    {
        var handlers = _serviceProvider.GetServices<IHandler<T>>().ToList();
        // if(handlers.Count == 0){ 
        //     await _notifications.Handle(new DomainNotification("Bus", $"Erro de configuração: Nenhum Handler de NEGÓCIO registrado para {typeof(T).Name}."));
        // }
        // FILTRO DE SUCESSO REAL:
        // Contamos quantos handlers NÃO são o log genérico.
        var temHandlerDeNegocio = handlers.Any(h => h.GetType().Name != typeof(LogComandoGenericoHandler<>).Name);

        if (!temHandlerDeNegocio)
        {
            await _notifications.Handle(new DomainNotification("Bus", $"Erro de configuração: Nenhum Handler de NEGÓCIO registrado para {typeof(T).Name}."));

#if DEBUG
            throw new InvalidOperationException($"[ARQUITETURA] Você esqueceu de registrar o Handler de NEGÓCIO para: {typeof(T).Name}");
#else
        return;
#endif
        }

        foreach (var handler in handlers)
        {
            await handler.Handle(comando);
            if (await _notifications.HasNotification()) break;
        }
    }
}
