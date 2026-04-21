using System.Text.Json.Serialization;
using ITB.Domain.Core.Exceptions;

namespace ITB.Domain.Entities;

public class Marca
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public bool Ativo { get; set; } = true;
    [JsonIgnore]
    public readonly IEnumerable<Veiculo> Veiculos;
    // [JsonIgnore]
    // private readonly List<Veiculo> _veiculos = new();
    // public virtual IReadOnlyCollection<Veiculo> Veiculos => _veiculos.AsReadOnly();
    public Marca(string nome) {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException("Nome é obrigatório.");
        Nome = nome;
    }
}
