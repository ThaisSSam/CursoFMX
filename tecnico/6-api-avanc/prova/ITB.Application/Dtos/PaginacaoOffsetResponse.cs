namespace ITB.Application.Dtos;

public class PaginacaoOffsetResponse<T>
{
    public IEnumerable<T> Dados { get; set; } = Enumerable.Empty<T>();
    public int PaginaAtual { get; set; }
    public int TotalRegistros { get; set; }
    public int TotalPaginas { get; set; }
}