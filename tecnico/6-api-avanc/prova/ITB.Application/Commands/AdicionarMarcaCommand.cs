using System;
using ITB.Domain.Core.Messages.Interfaces;


namespace ITB.Application.Commands;

public class AdicionarMarcaCommand: ICommand
{
    public string nome { get; set; } = string.Empty;
}
