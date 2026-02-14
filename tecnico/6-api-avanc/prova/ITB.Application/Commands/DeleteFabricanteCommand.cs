using ITB.Domain.Core.Messages;
using ITB.Domain.Core.Messages.Interfaces;

namespace ITB.Application.Commands;

public class DeletarFabricanteCommand : ICommand
{
    public int id { get; set; }

    public DeletarFabricanteCommand(int id)
    {
        this.id = id;
    }
}