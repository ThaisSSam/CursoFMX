namespace ITB.Infrastructure.Queries.Dtos;

public class MarcaVeiculoFlatDto
{
    public int MarcaId { get; set; }
    public string MarcaNome { get; set; } = string.Empty;
    public bool MarcaAtivo { get; set; }
    public int? VeiculoId { get; set; }
    public string? VeiculoPlaca { get; set; }
    public string? ModeloNome { get; set; }
    public int? VeiculoAno { get; set; }
}
