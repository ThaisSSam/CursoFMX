using ITB.Application.Commands;
using ITB.Domain.Core.Exceptions;
using ITB.Domain.Core.Messages;
using ITB.Domain.Core.Messages.Interfaces;
using ITB.Domain.Interfaces;

public class AtualizarModeloHandler : IHandler<AtualizarModeloCommand>
{
    private readonly IModeloRepository _modeloRepository;
    private readonly IMarcaRepository _marcaRepository;
    private readonly IUnitOfWork _uow;

    public AtualizarModeloHandler(
        IModeloRepository modeloRepository,
        IMarcaRepository marcaRepository,
        IUnitOfWork uow)
    {
        _modeloRepository = modeloRepository;
        _marcaRepository = marcaRepository;
        _uow = uow;
    }

    public async Task Handle(AtualizarModeloCommand command)
    {
        var modelo = await _modeloRepository.ObterPorIdAsync(command.Id);
        if (modelo == null) throw new DomainException("Modelo não encontrado.");

        bool marcaExiste = true;

        if (modelo.MarcaId != command.MarcaId)
        {
            marcaExiste = await _marcaRepository.ObterPorIdAsync(command.MarcaId) != null;
        }

        modelo.AtualizarDados(command.Nome, marcaExiste ? command.MarcaId : 0, command.Ativo);

        await _uow.Commit();

        // return new CommandResult(
        //     sucesso: true,
        //     mensagem: "Modelo atualizado com sucesso."
        // );
        // if (!await _uow.Commit())
        // {
        //     throw new DomainException("Ocorreu um erro ao salvar as alterações no banco de dados.");
        // }
    }
}