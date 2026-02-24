using System;
using ITB.Application.Commands;
using ITB.Domain.Core.Messages.Interfaces;
using ITB.Domain.Entities;
using ITB.Domain.Interfaces;

namespace ITB.Application.Handlers;

public class AdicionarMarcaHandler : IHandler<AdicionarMarcaCommand>
{
    private readonly IMarcaRepository _repository;

    public AdicionarMarcaHandler(IMarcaRepository repository) => _repository = repository; 

    public async Task Handle(AdicionarMarcaCommand command)
    {
        var marca = new Marca(command.nome)
        {
            Nome = command.nome,
        };
        await _repository.AdicionarAsync(marca);
    }
}
