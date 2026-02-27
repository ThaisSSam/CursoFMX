using System;
using ITB.Domain.Core.Messages.Interfaces;

namespace ITB.Domain.Core.Commands;

public class AtualizarVeiculoCommand : ICommand
{
    public int Id { get; set; } 
    public string Placa { get; set; } = string.Empty;
    public int Ano { get; set; }
    public int ModeloId { get; set; } 
}