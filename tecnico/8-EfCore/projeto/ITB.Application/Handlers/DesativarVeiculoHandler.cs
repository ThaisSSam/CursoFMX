using ITB.Application.Commands;
using ITB.Domain.Core.Exceptions;
using ITB.Domain.Core.Messages;
using ITB.Domain.Core.Messages.Interfaces;
using ITB.Domain.Interfaces;

public class DesativarVeiculoHandler : IHandler<DesativarVeiculoCommand>
{
    private readonly IVeiculoRepository _veiculoRepository;
    private readonly IUnitOfWork _uow;

    public DesativarVeiculoHandler(IVeiculoRepository veiculoRepository, IUnitOfWork uow)
    {
        _veiculoRepository = veiculoRepository;
        _uow = uow;
    }

    public async Task Handle(DesativarVeiculoCommand command)
    {
        var veiculo = await _veiculoRepository.ObterPorIdAsync(command.Id);
        if (veiculo == null) throw new DomainException("Veículo não encontrado.");

        veiculo.Desativar();
        await _veiculoRepository.AtualizarAsync(veiculo);
        await _uow.Commit();

        // return new CommandResult(
        //     sucesso: true,
        //     mensagem: "Veículo desativado com sucesso."
        // );
    }
}