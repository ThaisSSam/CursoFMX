using ITB.Domain.Core.Messages.Interfaces;

namespace ITB.Application.Commands;

public class DesativarMarcaCommand : ICommand
{
    public int Id { get; set; }
}
