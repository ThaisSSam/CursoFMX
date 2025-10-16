using System;
using System.ComponentModel.DataAnnotations;

namespace LojaApi.Infra.DTOs;

public class CriarProduto
{
    [Required(ErrorMessage = "O nome do produto é obrigatório.")]
    [StringLength(150, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 150 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O preço é obrigatório.")]
    [Range(0.01, 100000.00, ErrorMessage = "O preço deve ser entre 0.01 e 100,000.00.")]
    public decimal Preco { get; set; }

    [Range(0, 9999, ErrorMessage = "O estoque deve ser entre 0 e 9999.")]
    public int Estoque { get; set; }

    [Required(ErrorMessage = "O ID da categoria é obrigatório.")]
    public int CategoriaId { get; set; }
}
