using System;
using ITB.Domain.Core.Messages.Interfaces;

namespace ITB.Application.Commands;

public class AdicionarModeloCommand : ICommand
{
    public string nome { get; set;}= string.Empty;

    public int marcaId { get; set;}

    public bool ativo{ get; set;} = true;
}
