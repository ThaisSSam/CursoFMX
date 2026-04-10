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

    // Altere a linha do método para:
    public async System.Threading.Tasks.Task Handle(AdicionarMarcaCommand comando)
    {
        var novaMarca = new Marca(comando.Nome);
        await _marcaRepository.AdicionarAsync(novaMarca);
        await _uow.CommitAsync();
    }
}
