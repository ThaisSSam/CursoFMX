using Treinamento.Domain.Core.Commands;

namespace Treinamento.Domain.Aggregates.Sistema.Commands;

/// <summary>
/// Comando de exemplo para validar o barramento CQRS no boilerplate.
/// </summary>
public class PingCommand : Command
{
    public string Mensagem { get; set; } = "ping";
    public string? Resposta { get; set; }
}
