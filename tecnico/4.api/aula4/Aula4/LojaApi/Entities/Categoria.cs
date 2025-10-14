using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LojaApi.Entities;
[Table("TB_CATEGORIAS")]
public class Categoria
{
    [Key]
    [Column("id_categoria")]
    public int Id { get; set; }

    [Column("nome_categoria")]
    [Required(ErrorMessage = "Obrigatório")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "nome deve ter mais de 3 e menos de 100 caracteres")]
    public string? Nome { get; set; }

    [JsonIgnore]
    public ICollection<Produto> Produtos { get; set; } = new List<Produto>();
}
