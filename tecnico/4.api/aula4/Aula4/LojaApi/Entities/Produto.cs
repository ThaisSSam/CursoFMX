using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;

namespace LojaApi.Entities;
[Table("TB_PRODUTO")]
public class Produto
{
    [Key]
    [Column("id_produto")]
    public int Id { get; set; }
    [Column("nome_pproduto")]
    [Required(ErrorMessage = "O nome é obrigatório")]
    [StringLength(150, MinimumLength = 3, ErrorMessage = "O nome deve ter mais que 3 e menor que 150 caracteres")]
    public string Nome { get; set; }
    [Column("preco_produto", TypeName = "decimal(18,2)")]
    [Required(ErrorMessage = "Obrigatorio")]
    [Range(0.01, 1000000.00, ErrorMessage = "O preço deve ser maior que 0.01 e menor que 1000000.00")]
    public decimal Preco { get; set; }
    [Column("estoque_produto")]
    [Range(0, 9999, ErrorMessage = "O estoque deve ser maior que 0 e menor que 9999")]
    public int Estoque { get; set; }
    [Column("id_categoria")]
    [Required(ErrorMessage ="Obrigatório")]
    public int CategoriaId { get; set; }
    [ForeignKey("CategoriaId")]
    public Categoria? Categoria{ get; set; }
    
}
