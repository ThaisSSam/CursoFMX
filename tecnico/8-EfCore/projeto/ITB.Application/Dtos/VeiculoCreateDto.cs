using System;

namespace ITB.Application.Dtos;

public class VeiculoCreateDto
{
    public string Nome { get; set; } = string.Empty;
    public int MarcaId { get; set; }
    public int ModeloId { get; set; }
    public string Placa { get; set; } = string.Empty;
    public int Ano { get; set; }
}
