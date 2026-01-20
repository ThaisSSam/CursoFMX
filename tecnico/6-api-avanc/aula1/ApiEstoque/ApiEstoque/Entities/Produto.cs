using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiEstoque.Entities;

[Table("produtos")]
public class Produto {
    [Column("id")]
    public int Id { get; set; }
    [Column("nome")]
    public string? Nome { get; set; }
    [Column("preco")]
    public decimal Preco { get; set; }
    [Column("fabricanteid")]
    public int FabricanteId { get; set; }
    public Fabricante? Fabricante { get; set; }
}
