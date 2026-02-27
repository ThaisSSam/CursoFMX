using System;
using ITB.Application.Commands;
using ITB.Domain.Core.Messages.Interfaces;
using ITB.Domain.Entities;
using ITB.Domain.Interfaces;

namespace ITB.Application.Handlers;

public class AdicionarModeloHandler : IHandler<AdicionarModeloCommand>
{
    private readonly IModeloRepository _repository;

    public AdicionarModeloHandler (IModeloRepository repository)=> _repository=repository;

    public async Task Handle(AdicionarModeloCommand command)
    {
        var modelo = new Modelo(
            command.nome,
            command.marcaId,
            command.ativo
        );

        await _repository.AdicionarAsync(modelo);
    }
}
