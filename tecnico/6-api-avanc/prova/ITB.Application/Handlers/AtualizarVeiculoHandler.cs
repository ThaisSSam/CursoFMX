using System;
using ITB.Domain.Core.Commands;
using ITB.Domain.Core.Exceptions;
using ITB.Domain.Core.Messages.Interfaces;
using ITB.Domain.Interfaces;

namespace ITB.Application.Handlers;

public class AtualizarVeiculoHandler : IHandler<AtualizarVeiculoCommand>
{
    private readonly IVeiculoRepository _veiculoRepository;
    private readonly IModeloRepository _modeloRepository;

    private readonly IUnitOfWork _uow;

    public AtualizarVeiculoHandler(IVeiculoRepository veiculoRepository, IModeloRepository modeloRepository, IUnitOfWork uow)
    {
        _veiculoRepository = veiculoRepository;
        _modeloRepository = modeloRepository;
        _uow = uow;
    }

    public async Task Handle(AtualizarVeiculoCommand command)
    {
        var veiculo = await _veiculoRepository.ObterPorId(command.Id);
        if (veiculo == null) throw new DomainException("Veículo não encontrado.");

        // 1. Levantamento de Fatos (O Handler é apenas o Escrivão)
        var placaDuplicada = await _veiculoRepository.PlacaJaExiste(command.Placa, command.Id);
        
        bool modeloExiste = true; 
        
        // Otimização: Evitamos ir ao banco consultar a modelo se o usuário não mudou o ID
        if (veiculo.ModeloId != command.ModeloId)
        {
            modeloExiste = await _modeloRepository.ObterPorIdAsync(command.ModeloId) != null;
        }

        // 2. Aplicação das Intenções (A Entidade é o Juiz)
        veiculo.AtualizarDados(command.ModeloId, command.Ano);
        veiculo.AlterarPlaca(command.Placa, placaDuplicada);
        // veiculo.AlterarModelo(command.ModeloId, modeloExiste);

        // 3. Persistência
        // await _veiculoRepository.Atualizar(veiculo);  
        
        if(!await _uow.Commit())
        {
            // Se o EF Core não conseguir salvar (ex: banco caiu, timeout), o sistema avisa!
            throw new DomainException("Ocorreu um erro ao salvar as alterações no banco de dados.");
        }
    }
}