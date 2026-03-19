namespace ITB.Application.Dtos;

public class PaginacaoResponse<T>
{
    public IEnumerable<T> Dados { get; set; } = Enumerable.Empty<T>();
    public bool TemMaisPaginas { get; set; }
    public int? ProximoCursor { get; set; }
}