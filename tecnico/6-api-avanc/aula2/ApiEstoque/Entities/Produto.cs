using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiEstoque.Entities;

[Table("produtos")]
public class Produto {
    public int Id { get; set; }
    public string? Nome { get; set; }
    public decimal Preco { get; set; }
    public int FabricanteId { get; set; }
    public Fabricante? Fabricante { get; set; }
}
