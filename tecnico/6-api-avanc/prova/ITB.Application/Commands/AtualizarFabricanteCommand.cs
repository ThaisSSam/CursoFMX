using System.Text.Json.Serialization;
using ITB.Domain.Core.Messages.Interfaces;

namespace ITB.Application.Commands;

public class AtualizarFabricanteCommand : ICommand
{
    [JsonIgnore]
    public int Id { get; set; }
    public string nome { get; set; } = string.Empty;
    public string cnpj { get; set; } = string.Empty;
}
