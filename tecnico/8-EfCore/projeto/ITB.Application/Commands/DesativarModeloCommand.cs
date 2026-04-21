using ITB.Domain.Core.Messages.Interfaces;

namespace ITB.Application.Commands;

public class DesativarModeloCommand : ICommand
{
    public int Id { get; set; }
}
