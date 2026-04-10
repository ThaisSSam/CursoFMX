using ITB.Domain.Core.Messages.Interfaces;

namespace ITB.Application.Commands;

public class DeletarVeiculoCommand : ICommand
{
    public int Id { get; set; }
}
