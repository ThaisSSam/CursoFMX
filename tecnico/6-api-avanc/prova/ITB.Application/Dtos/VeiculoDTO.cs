using System;

namespace ITB.Application.Dtos;

public class VeiculoDTO
{
    public int Id { get; set; }
    public string Placa { get; set; } = string.Empty;
    public int Ano { get; set; }
    public bool Ativo { get; set; }
    public ModeloDTO? Modelo { get; set; }
}