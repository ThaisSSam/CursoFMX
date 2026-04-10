using ITB.Domain.Core.Messages.Interfaces;

namespace ITB.Application.Commands;

public class AdicionarVeiculoCommand : ICommand
{
    public string Nome { get; set; } = string.Empty;
    public int MarcaId { get; set; }
    public int ModeloId { get; set; }
    public string Placa { get; set; } = string.Empty;
    public int Ano { get; set; }
    public decimal PrecoCusto { get; set; }
    public decimal PrecoVenda { get; set; }
    public int CriadoPorId { get; set; }
    public int? IdGerado { get; set; }
}
