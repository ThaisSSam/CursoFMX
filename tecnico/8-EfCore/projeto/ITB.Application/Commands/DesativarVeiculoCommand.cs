using ITB.Domain.Core.Messages.Interfaces;

namespace ITB.Application.Commands;

public class DesativarVeiculoCommand : ICommand
{
  public int Id { get; set; }
}
