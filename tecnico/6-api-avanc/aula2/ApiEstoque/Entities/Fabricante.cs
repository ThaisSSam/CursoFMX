using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiEstoque.Entities;

[Table("fabricantes")]
public class Fabricante {
    [Column("id")]
    public int Id { get; set; }
    [Column("nome")]
    public string? Nome { get; set; }
    public ICollection<Produto> Produtos { get; set; } = new List<Produto>();
}