using System;

namespace ITB.Application.Dtos;

public class VeiculoReadDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int MarcaId { get; set; }
    public string MarcaNome { get; set; } = string.Empty;
    public string Modelo { get; set; } = string.Empty;
    public string Placa { get; set; } = string.Empty;
    public int Ano { get; set; }
}
