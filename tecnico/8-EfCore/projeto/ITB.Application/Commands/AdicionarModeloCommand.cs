using ITB.Domain.Core.Messages.Interfaces;

namespace ITB.Application.Commands;

public class AdicionarModeloCommand : ICommand
{
    public string Nome { get; set; }
    public int MarcaId { get; set; }
}
