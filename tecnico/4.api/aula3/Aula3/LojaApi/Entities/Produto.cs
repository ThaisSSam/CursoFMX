using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LojaApi.Entities{

    [Table("TB_PRODUTOS")]
    public class Produto
    {
        [Key]
        [Column("id_produto")]
        public int Id { get; set; }
        [Column("nome_produto")]
        [Required]
        [StringLength(150)]
        public string Nome { get; set; } = string.Empty;
        [Column("valor_produto")]
        [Required]
        public decimal Valor { get; set; } = 0.0m;
        [Column("descricao_produto")]
        [Required]
        [StringLength(500)]
        public string Descricao { get; set; } = string.Empty;
        [Column("estoque_produto")]
        [Required]
        public int Estoque { get; set; }
        [Column("deletado")]
        [Required]
        public bool Deletado { get; set; }
    }
}