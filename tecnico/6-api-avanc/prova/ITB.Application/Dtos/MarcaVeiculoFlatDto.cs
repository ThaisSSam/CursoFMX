using System;

namespace ITB.Application.Dtos;

public class MarcaVeiculoFlatDTO 
{ 
    // Dados da Marca 
    public int MarcaId { get; set; } 
    public string MarcaNome { get; set; } = string.Empty; 
    public bool MarcaAtivo { get; set; } 

    // Dados do Veículo (Podem ser nulos por causa do LEFT JOIN) 
    public int? VeiculoId { get; set; } 
    public string? VeiculoPlaca { get; set; } 
    public string? ModeloNome { get; set; } 
    public int? VeiculoAno { get; set; } 
}