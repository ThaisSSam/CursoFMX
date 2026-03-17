using System;
using ITB.Application.Commands;
using ITB.Application.Dtos;
using ITB.Domain.Core.Messages;
using ITB.Domain.Core.Messages.Interfaces;
using ITB.Domain.Interfaces;

namespace ITB.Application.Handlers;

public class DescontoHandler : IHandler<DescontoCommand>
{
    private readonly IVeiculoRepository _veiculoRepository;

    private readonly IUnitOfWork _uow;

    public DescontoHandler(IVeiculoRepository veiculoRepository, IUnitOfWork uow)
    {
        _veiculoRepository = veiculoRepository;
        _uow = uow;
    }

    public async Task<CommandResult> Handle(DescontoCommand comando)
    {
        // Validar o comando
        if (!comando.EhValido()) 
            return new CommandResult(false, "Dados inválidos", null);

        await _veiculoRepository.AplicarDescontoAsync(comando.nomeMarca);

        var sucesso = await _uow.CommitAsync();

        if (sucesso)
        {
            return new CommandResult(true, "Desconto aplicado com sucesso!", null);
        }

        return new CommandResult(false, "Erro ao aplicar desconto", null);
    }
}
