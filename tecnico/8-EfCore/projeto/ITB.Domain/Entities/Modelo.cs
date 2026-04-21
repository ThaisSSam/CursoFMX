using System.Text.Json.Serialization;
using ITB.Domain.Core.Exceptions;

namespace ITB.Domain.Entities;

public class Modelo
{
    public int Id { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public int MarcaId { get; private set; }
    public virtual Marca Marca { get; private set; }
    public bool Ativo { get; private set; }
    [JsonIgnore]
    public readonly IEnumerable<Veiculo> Veiculos;

    public Modelo(string nome, int marcaId, bool ativo)
    {
        AtualizarDados(nome, marcaId, ativo);
    }

    public void Desativar()
    {
        Ativo = false;
    }

    public void Ativar()
    {
        Ativo = true;
    }

    public void AtualizarDados(string nome, int marcaId, bool ativo)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException("Nome é obrigatório");

        if (marcaId <= 0)
            throw new DomainException("Marca inválida");
        
        Nome = nome;
        MarcaId = marcaId;
        Ativo = ativo;
    }
}
