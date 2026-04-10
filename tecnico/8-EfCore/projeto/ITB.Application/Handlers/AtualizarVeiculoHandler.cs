using ITB.Application.Commands;
using ITB.Domain.Core.Exceptions;
using ITB.Domain.Core.Messages;
using ITB.Domain.Core.Messages.Interfaces;
using ITB.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

public class AtualizarVeiculoHandler : IHandler<AtualizarVeiculoCommand>
{
    private readonly IVeiculoRepository _veiculoRepository;
    private readonly IMarcaRepository _marcaRepository;
    private readonly IUnitOfWork _uow;
    private readonly IDomainNotificationHandler<DomainNotification> _notifications;

    public AtualizarVeiculoHandler(
        IVeiculoRepository veiculoRepository,
        IMarcaRepository marcaRepository,
        IUnitOfWork uow,
        IDomainNotificationHandler<DomainNotification> notifications)
    {
        _veiculoRepository = veiculoRepository;
        _marcaRepository = marcaRepository;
        _uow = uow;
        _notifications = notifications;
    }

    public async Task Handle(AtualizarVeiculoCommand command)
    {
        var veiculo = await _veiculoRepository.ObterPorIdAsync(command.Id);
        if (veiculo == null) await _notifications.Handle(new DomainNotification("VeiculoId", "Veículo não encontrado."));

        var placaDuplicada = await _veiculoRepository.PlacaJaExiste(command.Placa, command.Id);

        bool marcaExiste = true;

        if (veiculo.MarcaId != command.MarcaId)
        {
            marcaExiste = await _marcaRepository.ObterPorIdAsync(command.MarcaId) != null;
        }

        veiculo.AtualizarDados(command.ModeloId, command.Ano);
        veiculo.AlterarPlaca(command.Placa, placaDuplicada);
        veiculo.AlterarMarca(command.MarcaId, marcaExiste);

        try
        {
            await _veiculoRepository.AtualizarAsync(veiculo);
            if (!await _uow.Commit())
            {
                await _notifications.Handle(new DomainNotification("Veiculo", "Ocorreu um erro ao salvar as alterações."));
            }

            // return new CommandResult(
            //     sucesso: true,
            //     mensagem: "Veículo atualizado com sucesso"
            // );
        } catch (DbUpdateConcurrencyException)
        {
            await _notifications.Handle(new DomainNotification("Veiculo", "Este veículo foi modificado por outro usuário enquanto você o editava. Recarrega a página."));
            // return new CommandResult(
            //     sucesso: false,
            //     mensagem: "Este veículo foi modificado por outro usuário enquanto você o editava. Recarrega a página."
            // );
        }
        // if (!await _uow.Commit())
        // {
        //     throw new DomainException("Ocorreu um erro ao salvar as alterações no banco de dados.");
        // }
    }
}