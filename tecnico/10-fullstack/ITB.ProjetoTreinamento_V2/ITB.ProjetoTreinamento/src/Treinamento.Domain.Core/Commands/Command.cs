using Treinamento.Domain.Core.Events;

namespace Treinamento.Domain.Core.Commands;

public class Command : Message, ICommand
{
    public DateTime DataHora { get; private set; }

    public Command() => DataHora = DateTime.Now;
}
