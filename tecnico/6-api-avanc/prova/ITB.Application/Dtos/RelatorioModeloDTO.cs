using System;

namespace ITB.Application.Dtos;

public class RelatorioModeloDTO
{
    public int id { get; set; }
    public string nome_modelo { get; set; } = string.Empty;

    public int quantidade { get; set; } = 0;

}
