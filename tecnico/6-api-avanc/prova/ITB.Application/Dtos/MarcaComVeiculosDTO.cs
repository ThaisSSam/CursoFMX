using System;
using ITB.Application.Dtos;
using ITB.Application.Interfaces;
using ITB.Domain.Entities;

namespace ITB.Infrastructure.Queries;

public class MarcaComVeiculosDTO 
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public bool Ativo { get; set; }
    public List<VeiculoSimplesDTO> Veiculos { get; set; } = new();
}