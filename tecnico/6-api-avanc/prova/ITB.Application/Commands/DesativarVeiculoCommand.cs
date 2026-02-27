using System;
using ITB.Domain.Core.Messages.Interfaces;

namespace ITB.Domain.Core.Commands;

public class DesativarVeiculoCommand : ICommand
{
    public int Id { get; set; }
}