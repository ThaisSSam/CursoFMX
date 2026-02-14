using ITB.Application.Commands;
using ITB.Domain.Core.Messages.Interfaces;
using ITB.Domain.Interfaces;

public class DeletarFabricanteHandler : IHandler<DeletarFabricanteCommand>
{
    private readonly IFabricanteRepository _repository;

    public DeletarFabricanteHandler(IFabricanteRepository repository) => _repository = repository;

    public async Task Handle(DeletarFabricanteCommand comando)
    {
        var fabricante = await _repository.ObterPorIdAsync(comando.id);

        if (fabricante != null)
        {
            await _repository.RemoverAsync(comando.id);
        }
    }
}