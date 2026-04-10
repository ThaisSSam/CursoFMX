using ITB.Application.Commands;
using ITB.Domain.Core.Messages;
using ITB.Domain.Core.Messages.Interfaces;
using ITB.Domain.Entities;
using ITB.Domain.Interfaces;

namespace ITB.Application.Handlers;

public class AdicionarMarcaHandler : IHandler<AdicionarMarcaCommand>
{
    private IMarcaRepository _repository;
    private IUnitOfWork _uow;
    public AdicionarMarcaHandler(IMarcaRepository repository, IUnitOfWork uow)
    {
        _repository = repository;
        _uow = uow;
    }

    public async Task Handle(AdicionarMarcaCommand comando)
    {
        var novaMarca = new Marca(comando.Nome);
        await _repository.AdicionarAsync(novaMarca);
        await _uow.Commit();
        // return new CommandResult(
        //     sucesso: true,
        //     mensagem: "Marca cadastrada com sucesso!",
        //     dados: novaMarca.Id
        // );
    }
}
