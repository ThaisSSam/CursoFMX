using System;
using ITB.Application.Commands;
using ITB.Domain.Core.Messages;
using ITB.Domain.Core.Messages.Interfaces;
using ITB.Domain.Entities;
using ITB.Domain.Interfaces;

namespace ITB.Application.Handlers;

public class AdicionarVeiculoHandler : IHandler<AdicionarVeiculoCommand>
{
    private readonly IVeiculoRepository _veiculoRepository;

    private readonly IUnitOfWork _uow;

    // public AdicionarVeiculoHandler(IVeiculoRepository veiculoRepository) => _veiculoRepository = veiculoRepository;

    public AdicionarVeiculoHandler(IVeiculoRepository veiculoRepository, IUnitOfWork uow)
    {
        _veiculoRepository = veiculoRepository;
        _uow = uow;
    }

    // public async Task Handle(AdicionarVeiculoCommand command)
    // {
    //     var veiculo = new Veiculo(
    //         command.placa, 
    //         command.ano, 
    //         command.modeloId
    //     );
    //     await _veiculoRepository.AdicionarAsync(veiculo);
    // }

    public async Task<CommandResult> Handle(AdicionarVeiculoCommand comando)
    {
        var novaVeiculo = new Veiculo(comando.id, comando.placa);
        _veiculoRepository.AdicionarAsync(novaVeiculo);
        await _uow.CommitAsync();

        return new CommandResult(
            sucesso: true,
            mensagem: "Veiculo cadastrada com sucesso!",
            dados: new {id = novaVeiculo.Id}
        );
    }
}
