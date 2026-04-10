using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BulkInsertDemo;

[Table("veiculo")]
[Index(nameof(Placa), IsUnique = true)]
public class Veiculo
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }
    [Column("placa")]
    public string Placa { get; set; } = string.Empty;
    [Column("modelo")]
    public string Modelo { get; set; } = string.Empty;
    [Column("preco")]
    public decimal Preco { get; set; }
}
