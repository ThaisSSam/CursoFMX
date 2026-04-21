using ITB.Application.Commands;
using ITB.Domain.Core.Messages;
using ITB.Domain.Core.Messages.Interfaces;
using ITB.Domain.Entities;
using ITB.Domain.Interfaces;

namespace ITB.Application.Handlers;

public class DescontoMarcaVeiculoHandler : IHandler<DescontoMarcaVeiculoCommand>
{
    private IVeiculoRepository _repository;
    private IUnitOfWork _uow;
    public DescontoMarcaVeiculoHandler(IVeiculoRepository repository, IUnitOfWork uow)
    {
        _repository = repository;
        _uow = uow;
    }

    public async Task Handle(DescontoMarcaVeiculoCommand comando)
    {
        var linhasAfetadas = await _repository.DescontoEmMassaPorMarca(comando.Marca, comando.PercentualDesconto, comando.Ano);
        await _uow.Commit();
        // return new CommandResult(
        //     sucesso: true,
        //     mensagem: "Desconto efetuado com sucesso!",
        //     dados: linhasAfetadas
        // );
    }
}
