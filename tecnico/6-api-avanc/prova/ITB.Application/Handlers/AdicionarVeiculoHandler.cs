using System;
using ITB.Application.Commands;
using ITB.Domain.Core.Messages.Interfaces;
using ITB.Domain.Entities;
using ITB.Domain.Interfaces;

namespace ITB.Application.Handlers;

public class AdicionarVeiculoHandler : IHandler<AdicionarVeiculoCommand>
{
    private readonly IVeiculoRepository _repository;

    public AdicionarVeiculoHandler(IVeiculoRepository repository) => _repository = repository;

    public async Task Handle(AdicionarVeiculoCommand command)
    {
        var veiculo = new Veiculo(
            command.placa, 
            command.ano, 
            command.modeloId
        );
        await _repository.AdicionarAsync(veiculo);
    }
}
