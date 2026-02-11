using System.Threading.Tasks;
using ITB.Domain.Interfaces;
using ITB.Domain.Core.Messages; 
using ITB.Application.Commands; 

namespace ITB.Application.Handlers;

public class ExcluirFabricanteHandler : IHandler<ExcluirFabricanteCommand>
{
    private readonly IFabricanteRepository _repository;

    public ExcluirFabricanteHandler(IFabricanteRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(ExcluirFabricanteCommand command)
    {
        await _repository.RemoverAsync(command.Id);
    }
}