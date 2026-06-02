namespace Treinamento.API.Controllers; 

public sealed class TarefaRequest
{
    public required string Nome { get; init; }
    public required string Situacao { get; init; } 
    public required string Prioridade { get; init; }
    public required int UsuarioId { get; init; }
    public required DateTime DataCriacao { get; init; }
}