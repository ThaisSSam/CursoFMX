namespace Treinamento.Domain.Commands;

public class CriarTarefaCommand
{
    public string Nome { get; set; } = string.Empty;
    public int Prioridade { get; set; }
    public int Situacao { get; set; }
    public int UsuarioId { get; set; }
}