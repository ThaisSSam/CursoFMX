using System;

namespace ITB.Application.Dtos;

public class VeiculoSimplesDTO 
{ 
    public int Id { get; set; } 
    public string Placa { get; set; } = string.Empty; 
    public string Modelo { get; set; } = string.Empty; 
    public int Ano { get; set; } 
} 
