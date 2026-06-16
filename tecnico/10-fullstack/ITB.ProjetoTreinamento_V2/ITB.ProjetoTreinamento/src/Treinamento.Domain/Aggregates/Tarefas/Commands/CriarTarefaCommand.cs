using System.Text.Json.Serialization;

namespace Treinamento.Domain.Commands;

public class CriarTarefaCommand
{
    public string Nome { get; set; } = string.Empty;
    [JsonConverter(typeof(JsonConversorStringOuInt))]
    public int Prioridade { get; set; }

    [JsonConverter(typeof(JsonConversorStringOuInt))]  
    public int Situacao { get; set; }
    public int UsuarioId { get; set; }
}