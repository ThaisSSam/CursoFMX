using System;
using ITB.Application.Commands;
using ITB.Domain.Core.Messages;
using ITB.Domain.Core.Messages.Interfaces;
using ITB.Domain.Entities;
using ITB.Domain.Interfaces;

namespace ITB.Application.Handlers;

public class AdicionarMarcaHandler : IHandler<AdicionarMarcaCommand>
{
    private readonly IMarcaRepository _marcaRepository;
    private readonly IUnitOfWork _uow;
    public AdicionarMarcaHandler(IMarcaRepository marcaRepository, IUnitOfWork uow)
    {
        _marcaRepository = marcaRepository;
        _uow = uow;
    }

    // public AdicionarMarcaHandler(IMarcaRepository marcaRepository) => _marcaRepository = marcaRepository; 

    // public async Task Handle(AdicionarMarcaCommand command)
    // {
    //     var marca = new Marca(command.Nome)
    //     {
    //         Nome = command.Nome,
    //     };
    //     await _repository.AdicionarAsync(marca);
    // }

    public async Task<CommandResult> Handle(AdicionarMarcaCommand comando)
    {
        var novaMarca = new Marca(comando.Nome);
        _marcaRepository.AdicionarAsync(novaMarca);
        await _uow.CommitAsync();

        return new CommandResult(
            sucesso: true,
            mensagem: "Marca cadastrada com sucesso!",
            dados: new {id = novaMarca.Id}
        );
    }
}
