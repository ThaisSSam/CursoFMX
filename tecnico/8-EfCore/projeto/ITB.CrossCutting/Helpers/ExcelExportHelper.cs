using ClosedXML.Excel;

public static class ExcelExportHelper
{
    public static byte[] ExportarParaExcel<T>(IEnumerable<T> dados, List<string> colunasPermitidas, string nomePlanilha = "Relatorio")
    {
        //Abertura do arquivo em memória
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add(nomePlanilha);

        //Uso de Reflection para ler a classe
        var propriedades = typeof(T).GetProperties()
            .Where(p => colunasPermitidas.Contains(p.Name))
            .ToList();

        //O Índice 1 do Excel
        for (int i = 0; i < propriedades.Count; i++)
        {
            var celula = worksheet.Cell(1, i + 1);
            celula.Value = propriedades[i].Name;
            celula.Style.Font.Bold = true;
            celula.Style.Fill.BackgroundColor = XLColor.LightGray;
        }

        // 2. Preenchimento dos Dados
        var linhaAtual = 2;
        foreach (var item in dados)
        {
            for (int i = 0; i < propriedades.Count; i++)
            {
                var valor = propriedades[i].GetValue(item);
                var celula = worksheet.Cell(linhaAtual, i + 1);

                if (valor != null)
                {
                    // Passa o valor numérico real para o Excel
                    celula.Value = XLCellValue.FromObject(valor);

                    // Se o valor for dinheiro (decimal ou double), formata a célula na hora!
                    if (valor is decimal || valor is double)
                    {
                        // As aspas duplas forçam o Excel a desenhar o "R$" literalmente.
                        // O padrão #,##0.00 diz ao Excel para colocar ponto de milhar e 2 casas decimais.
                        celula.Style.NumberFormat.Format = "\"R$\" #,##0.00";
                    }
                }
                else
                {
                    celula.Value = "";
                }
            }
            linhaAtual++;
        }

        worksheet.Columns().AdjustToContents();

        //A Mágica do MemoryStream
        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}
