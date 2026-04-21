using ITB.Application.Commands;
using ITB.Domain.Core.Messages;
using ITB.Domain.Core.Messages.Interfaces;
using ITB.Domain.Interfaces;

namespace ITB.Application.Handlers;

public class DeletarMarcaHandler : IHandler<DeletarMarcaCommand>
{
    private IMarcaRepository _repository;
    private IUnitOfWork _uow;
    public DeletarMarcaHandler(IMarcaRepository repository, IUnitOfWork uow)
    {
        _repository = repository;
        _uow = uow;
    }

    public async Task Handle(DeletarMarcaCommand comando)
    {
        var marca = await _repository.ObterPorIdAsync(comando.Id);
        _repository.RemoverAsync(marca);
        await _uow.Commit();

        // return new CommandResult(
        //     sucesso: true,
        //     mensagem: "Marca deletado com sucesso."
        // );
    }
}
