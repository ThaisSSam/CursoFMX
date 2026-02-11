using ITB.Domain.Core.Messages;

namespace ITB.Application.Commands;

public class ExcluirFabricanteCommand : ICommand
{
    public int Id { get; set; }

    public ExcluirFabricanteCommand(int id)
    {
        Id = id;
    }
}