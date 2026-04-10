using ITB.Domain.Core.Messages.Interfaces;

namespace ITB.Application.Commands;

public class DescontoMarcaVeiculoCommand : ICommand
{
    public int Ano { get; set; }
    public decimal PercentualDesconto { get; set; }
    public string Marca { get; set; } = string.Empty;
}
