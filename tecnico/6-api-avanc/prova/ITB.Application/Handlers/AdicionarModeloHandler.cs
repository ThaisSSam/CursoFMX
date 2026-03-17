using System;
using ITB.Application.Commands;
using ITB.Domain.Core.Messages;
using ITB.Domain.Core.Messages.Interfaces;
using ITB.Domain.Entities;
using ITB.Domain.Interfaces;

namespace ITB.Application.Handlers;

public class AdicionarModeloHandler : IHandler<AdicionarModeloCommand>
{
    private readonly IModeloRepository _modeloRepository;
    private readonly IUnitOfWork _uow;
    // public AdicionarModeloHandler(IModeloRepository modeloRepository) => _modeloRepository = modeloRepository; 

    public AdicionarModeloHandler(IUnitOfWork uow) => _uow = uow;

    // public async Task Handle(AdicionarModeloCommand command)
    // {
    //     var modelo = new Modelo(
    //         command.nome,
    //         command.modeloId,
    //         command.ativo
    //     );

    //     await _repository.AdicionarAsync(modelo);
    // }

    public async Task<CommandResult> Handle(AdicionarModeloCommand comando)
    {
        var novaModelo = new Modelo(comando.nome, comando.marcaId, comando.ativo);
        _modeloRepository.AdicionarAsync(novaModelo);
        await _uow.CommitAsync();

        return new CommandResult(
            sucesso: true,
            mensagem: "Modelo cadastrada com sucesso!",
            dados: new {id = novaModelo.Id}
        );
    }
}
