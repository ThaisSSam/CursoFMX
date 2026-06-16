using System;
using Treinamento.Domain.Aggregates.Tarefa;

namespace Treinamento.Domain.Commands;

using System.Text.Json.Serialization;

public class EditarTarefaCommand
{
    public int Id { get; set; }
    public string Nome { get; set; } = "";

    [JsonConverter(typeof(JsonConversorStringOuInt))] 
    public int Prioridade { get; set; }

    [JsonConverter(typeof(JsonConversorStringOuInt))] 
    public int Situacao { get; set; }

    public int UsuarioId { get; set; }
}