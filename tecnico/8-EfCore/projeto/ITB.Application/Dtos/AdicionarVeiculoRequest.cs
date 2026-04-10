
namespace ITB.Application.Dtos;

public class AdicionarVeiculoRequest
{
    public string Nome { get; set; } = string.Empty;
    public int MarcaId { get; set; }
    public int ModeloId { get; set; }
    public string Placa { get; set; } = string.Empty;
    public int Ano { get; set; }
    public decimal PrecoCusto { get; set; }
    public decimal PrecoVenda { get; set; }
}
