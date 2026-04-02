using System;
using ITB.Application.Dtos;
using ITB.Application.Interfaces;
using ITB.Application.Queries;
using ITB.CrossCutting.Helpers;

namespace ITB.Application.Handlers;

public class ExportarVeiculosExcelQueryHandler
{
  private readonly IVeiculoQuery _veiculoQuery;

  public ExportarVeiculosExcelQueryHandler(IVeiculoQuery veiculoQuery)
  {
    _veiculoQuery = veiculoQuery;
  }

  public async Task<ArquivoExportacaoDto> Handle(ExportarVeiculosExcelQuery request)
  {
    var veiculos = await _veiculoQuery.ObterTodosAsync();

    // Os nomes aqui DEVEM ser idênticos aos nomes das propriedades do seu VeiculoDto
    var colunas = new List<string>
        {
            "Id",
            "Modelo",
            "Placa",
            "Ano",
            "PrecoVenda"
        };

    // Regra de Negócio Dinâmica: O Gerente vê o custo!
    if (request.CargoUsuarioLogado == "Gerente")
    {
      colunas.Add("PrecoCusto");
    }

    var bytesExcel = ExcelExportHelper.ExportarParaExcel(veiculos, colunas, "Estoque");

    return new ArquivoExportacaoDto
    {
      Conteudo = bytesExcel,
      NomeArquivo = $"Relatorio_Veiculos_{DateTime.Now:ddMMyyyy}.xlsx",

      //O MIME Type
      ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
    };
  }
}
