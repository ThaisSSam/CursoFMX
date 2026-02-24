using System;

namespace ITB.Domain.Entities;

public class Marca
{
    public int Id { get; set;}

    public string Nome { get; set;} = string.Empty;

    private readonly List<Veiculo> _veiculos = new();

    public virtual IReadOnlyCollection<Veiculo> Veiculos => _veiculos.AsReadOnly();

    public Marca(string nome){Nome = nome;}
}
