using System;
using ITB.Domain.Core.Commands;
using ITB.Domain.Core.Exceptions;
using ITB.Domain.Core.Messages.Interfaces;
using ITB.Domain.Interfaces;

namespace ITB.Application.Handlers;

public class DesativarVeiculoHandler : IHandler<DesativarVeiculoCommand>
{
    private readonly IVeiculoRepository _veiculoRepository;

    public DesativarVeiculoHandler(IVeiculoRepository veiculoRepository)
    {
        _veiculoRepository = veiculoRepository;
    }

    public async Task Handle(DesativarVeiculoCommand command)
    {
        var veiculo = await _veiculoRepository.ObterPorId(command.Id);
        if (veiculo == null) throw new DomainException("Veículo não encontrado.");

        veiculo.Desativar();
        await _veiculoRepository.Atualizar(veiculo);
    }
}