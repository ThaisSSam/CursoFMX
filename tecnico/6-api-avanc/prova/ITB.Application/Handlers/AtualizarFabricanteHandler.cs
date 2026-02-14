using ITB.Application.Commands;
using ITB.Domain.Core.Messages.Interfaces;
using ITB.Domain.Interfaces;

public class AtualizarFabricanteHandler : IHandler<AtualizarFabricanteCommand>
{
    private readonly IFabricanteRepository _repository;

    public AtualizarFabricanteHandler(IFabricanteRepository repository) => _repository = repository;

    public async Task Handle(AtualizarFabricanteCommand comando)
    {
        // Busca o fabricante existente no banco pelo ID que veio no comando
        var fabricante = await _repository.ObterPorIdAsync(comando.Id);

        if (fabricante != null)
        {
            fabricante.nome = comando.nome;
            fabricante.cnpj = comando.cnpj;

            await _repository.AtualizarAsync(fabricante);
        }
    }
}
