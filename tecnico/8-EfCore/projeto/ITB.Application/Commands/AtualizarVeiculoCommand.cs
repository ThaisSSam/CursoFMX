using System;
using ITB.Domain.Core.Messages.Interfaces;

namespace ITB.Application.Commands;

public class AtualizarVeiculoCommand : ICommand
{
    public string Nome { get; set; } = string.Empty;
    public int Id { get; set; } 
    public int ModeloId { get; set; }
    public string Placa { get; set; } = string.Empty;
    public int Ano { get; set; }
    public int MarcaId { get; set; }
    public decimal PrecoVenda { get; set; }
    public decimal PrecoCusto { get; set; }
    public int IdGerado { get; set; }
}
