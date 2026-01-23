using System;
using ApiEstoque.Entities;

namespace ApiEstoque.Infra.DTOs;

public class ProdutoDto
{
    public int Id {get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal Preco { get; set; } = decimal.Zero;
    public int FabricanteId { get; set; }
    //public string NomeFabricante { get; set; } = string.Empty;
    public FabricanteDto? FabricanteDetalhe {get; set;}
}
