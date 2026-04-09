using System.Threading.Tasks;
using ITB.Application.Commands;
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

    // A assinatura deve ser EXATAMENTE esta para satisfazer a IHandler
    public async System.Threading.Tasks.Task Handle(DescontoCommand comando)
    {
        // Sua lógica de desconto aqui
        if (comando == null) return;

        await _veiculoRepository.AplicarDescontoAsync(comando.nomeMarca);
        await _uow.CommitAsync();
    }
}