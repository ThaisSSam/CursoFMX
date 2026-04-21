using ITB.Application.Commands;
using ITB.Domain.Core.Exceptions;
using ITB.Domain.Core.Messages;
using ITB.Domain.Core.Messages.Interfaces;
using ITB.Domain.Entities;
using ITB.Domain.Interfaces;

namespace ITB.Application.Handlers;

public class AdicionarModeloHandler : IHandler<AdicionarModeloCommand>
{
    private IModeloRepository _repository;
    private IMarcaRepository _marcaRepository;
    private readonly IUnitOfWork _uow;

    public AdicionarModeloHandler(IModeloRepository repository, IMarcaRepository marcaRepository, IUnitOfWork uow)
    {
        _repository = repository;
        _marcaRepository = marcaRepository;
        _uow = uow;
    }

    public async Task Handle(AdicionarModeloCommand comando)
    {
        var marcaExiste = await _marcaRepository.ObterPorIdAsync(comando.MarcaId) is not null;

        var modelo = new Modelo(comando.Nome, marcaExiste ? comando.MarcaId : 0, true);
        await _repository.AdicionarAsync(modelo);

        await _uow.Commit();

        // return new CommandResult(
        //     sucesso: true,
        //     mensagem: "Modelo adicionado com sucesso."
        // );
    }
}
