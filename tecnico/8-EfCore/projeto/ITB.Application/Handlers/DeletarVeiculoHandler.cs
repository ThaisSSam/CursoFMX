using ITB.Application.Commands;
using ITB.Domain.Core.Messages;
using ITB.Domain.Core.Messages.Interfaces;
using ITB.Domain.Interfaces;

namespace ITB.Application.Handlers;

public class DeletarVeiculoHandler : IHandler<DeletarVeiculoCommand>
{
    private IVeiculoRepository _repository;
    private IUnitOfWork _uow;
    public DeletarVeiculoHandler(IVeiculoRepository repository, IUnitOfWork uow)
    {
        _repository = repository;
        _uow = uow;
    }

    public async Task Handle(DeletarVeiculoCommand comando)
    {
        var veiculo = await _repository.ObterPorIdAsync(comando.Id);
        _repository.RemoverAsync(veiculo);
        await _uow.Commit();

        // return new CommandResult(
        //     sucesso: true,
        //     mensagem: "Veículod deletado com sucesso."
        // );
    }
}
