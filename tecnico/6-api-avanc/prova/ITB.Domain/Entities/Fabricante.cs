using System;

namespace ITB.Domain.Entities;

public class Fabricante
{
    public int id { get; set; }
    public string nome { get; set; } = string.Empty;

    public string? cnpj { get; set; } = string.Empty;
}
