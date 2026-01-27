using System;
using System.ComponentModel.DataAnnotations;

namespace ApiEstoque.Infra.DTOs;

public class CriarFabricanteDto
{
    public decimal Id{ get; set;}

    [Required(ErrorMessage = "O nome é obrigatório")]
    [StringLength(100)]
    public string Nome { get; set;} = string.Empty;
}
