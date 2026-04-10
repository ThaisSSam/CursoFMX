using ITB.Domain.Core.Messages.Interfaces;

namespace ITB.Application.Commands;

public class AtualizarModeloCommand : ICommand
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public int MarcaId { get; set; }
    public bool Ativo { get; set; }
}
