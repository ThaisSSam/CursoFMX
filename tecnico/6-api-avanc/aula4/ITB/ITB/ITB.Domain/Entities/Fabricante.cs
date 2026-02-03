using System;

namespace ITB.Domain.Entities;

public class Fabricante
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;

    public string? Cnpj { get; set; } = string.Empty;
}
