using ITB.Domain.Core.Messages.Interfaces;

namespace ITB.Application.Commands;

public class DeletarMarcaCommand : ICommand
{
    public int Id { get; set; }
}
