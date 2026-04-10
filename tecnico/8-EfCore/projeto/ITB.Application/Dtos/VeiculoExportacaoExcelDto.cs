using System;

namespace ITB.Application.Dtos;

public class VeiculoExportacaoExcelDto
{
    public int Id { get; set; }
    public string Modelo { get; set; } = string.Empty;
    public int Ano { get; set; }
    public string Placa { get; set; } = string.Empty;
    public string Marca { get; set; } = string.Empty;
    public decimal PrecoCusto { get; set; }
    public decimal PrecoVenda { get; set; }
    public decimal Lucro { get; set; }
}
