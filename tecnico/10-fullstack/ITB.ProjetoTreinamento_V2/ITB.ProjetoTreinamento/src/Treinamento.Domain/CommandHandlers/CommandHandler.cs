using Treinamento.Domain.Core.Bus;
using Treinamento.Domain.Core.Interfaces;
using Treinamento.Domain.Core.Notifications;
using Treinamento.Domain.Core.Validacao;

namespace Treinamento.Domain.CommandHandlers;

public abstract class CommandHandler(
    IUnitOfWork uow,
    IBus bus,
    IDomainNotificationHandler<DomainNotification> notifications)
{
    private readonly IUnitOfWork _uow = uow;
    private readonly IBus _bus = bus;
    private readonly IDomainNotificationHandler<DomainNotification> _notifications = notifications;

    protected async Task NotifyErrorsValidations(ResultadoValidacaoDominio resultadoValidacao, CancellationToken cancellationToken = default)
    {
        foreach (var erro in resultadoValidacao.Erros)
        {
            await _bus.RaiseEvent(new DomainNotification(erro.NomePropriedade ?? string.Empty, erro.MensagemErro), cancellationToken);
        }
    }

    protected async Task<bool> SalvarAlteracoesPendentesAsync(CancellationToken cancellationToken = default)
    {
        if (await _notifications.HasNotification()) return false;
        var commandResponse = await _uow.SalvarAlteracoesPendentesAsync(cancellationToken);
        if (commandResponse.Success) return true;

        var mensagemErro = !string.IsNullOrWhiteSpace(commandResponse.ErrorMessage)
            ? commandResponse.ErrorMessage
            : "Ocorreu um erro ao salvar os dados no banco de dados.";

        await _bus.RaiseEvent(new DomainNotification("Commit", mensagemErro), cancellationToken);
        return false;
    }

    protected async Task<bool> Commit(CancellationToken cancellationToken = default)
    {
        if (await _notifications.HasNotification()) return false;
        var commandResponse = await _uow.Commit(cancellationToken);
        if (commandResponse.Success) return true;

        var mensagemErro = !string.IsNullOrWhiteSpace(commandResponse.ErrorMessage)
            ? commandResponse.ErrorMessage
            : "Ocorreu um erro ao salvar os dados no banco de dados.";

        await _bus.RaiseEvent(new DomainNotification("Commit", mensagemErro), cancellationToken);
        return false;
    }

    protected async Task<bool> HasNotifications() => await _notifications.HasNotification();
}
