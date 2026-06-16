using Treinamento.Domain.Core.Commands;
namespace Treinamento.Domain.Aggregates.Sistema.Commands;

public class PingCommand : Command
{
    public string Mensagem { get; set; } = "ping";
    public string? Resposta { get; set; }
}
