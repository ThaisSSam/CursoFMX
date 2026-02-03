using System;

namespace ITB.Domain.Entities;

public class Produto
{
    public int id { get; set; }
    public string? nome { get; set; }
    public decimal preco { get; set; }
    public int fabricanteId { get; set; }
    public Fabricante? fabricante { get; set; }
}
