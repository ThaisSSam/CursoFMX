using System;
using System.ComponentModel.DataAnnotations;

namespace ApiEstoque.Infra.DTOs;

public class CriarProdutoDto
{
    public decimal Id{ get; set;}

    [Required(ErrorMessage = "O nome é obrigatório")]
    [StringLength(100)]
    public string Nome { get; set;} = string.Empty;

    [Required(ErrorMessage = "O preço é obrigatório")]
    public decimal Preco{ get; set;} = decimal.Zero;

    [Required(ErrorMessage = "Insira o código do fabricante")]
    public int FabricanteId { get; set;} = 0;
}
