using System;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace ITB.Application.Dtos;

public class VeiculoExportacaoExcelDto
{
  public int Id { get; set; }
  public string Modelo { get; set; }
  public string Placa { get; set; }
  public int Ano { get; set; }
  public string Marca { get; set; }
  public decimal PrecoCusto { get; set; }
  public decimal PrecoVenda { get; set; }
  public decimal Lucro { get; set; }
}
