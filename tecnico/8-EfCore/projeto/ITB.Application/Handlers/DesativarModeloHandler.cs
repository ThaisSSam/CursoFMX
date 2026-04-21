using ITB.Application.Commands;
using ITB.Domain.Core.Exceptions;
using ITB.Domain.Core.Messages;
using ITB.Domain.Core.Messages.Interfaces;
using ITB.Domain.Interfaces;

public class DesativarModeloHandler : IHandler<DesativarModeloCommand>
{
    private readonly IModeloRepository _modeloRepository;
    private readonly IUnitOfWork _uow;

    public DesativarModeloHandler(IModeloRepository modeloRepository, IUnitOfWork uow)
    {
        _modeloRepository = modeloRepository;
        _uow = uow;
    }

    public async Task Handle(DesativarModeloCommand command)
    {
        var modelo = await _modeloRepository.ObterPorIdAsync(command.Id);
        if (modelo == null) throw new DomainException("Modelo não encontrado.");

        modelo.Desativar();

        await _uow.Commit();

        // return new CommandResult(
        //     sucesso: true,
        //     mensagem: "Modelo desativado com sucesso."
        // );
    }
}