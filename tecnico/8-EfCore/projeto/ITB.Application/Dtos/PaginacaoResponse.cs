using System;

namespace ITB.Application.Dtos;

public class PaginacaoResponse
{
    public int TemMaisPaginas { get; set; }
    public int ProximoCursor { get; set; }
    public object? Dados { get; set; }
}
