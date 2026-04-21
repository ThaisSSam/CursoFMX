namespace ITB.Application.Dtos;

public class VeiculoListagemDto
{
    public int Id { get; set; }
    public string Placa { get; set; } = string.Empty;
    public string Modelo { get; set; } = string.Empty;
    public string Marca { get; set; } = string.Empty;
    public int Ano { get; set; }
    public decimal PrecoVenda { get; set; }
    public decimal PrecoCusto { get; set; }
}
