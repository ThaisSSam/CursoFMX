using System;

namespace LojaApi.Infra.DTOs;

public class ProdutoDetalhado
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public decimal Preco { get; set; }
    public int Estoque { get; set; }
    public int CategoriaId { get; set; }
}
