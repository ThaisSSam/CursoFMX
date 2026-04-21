using ITB.Domain.Core.Messages.Interfaces;

namespace ITB.Application.Commands;

public class AdicionarMarcaCommand : ICommand
{
    public string Nome { get; set; } = string.Empty;
}
