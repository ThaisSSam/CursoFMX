using System;
using ITB.Domain.Entities;

namespace ITB.Application.Dtos;
public class ProdutoCreateDto {
    public string nome { get; set; } = string.Empty;
    public decimal preco { get; set; }
    public int fabricanteId { get; set; }
}