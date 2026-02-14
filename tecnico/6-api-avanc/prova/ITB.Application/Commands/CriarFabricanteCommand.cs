using System;
using ITB.Domain.Core.Messages;
using ITB.Domain.Core.Messages.Interfaces;

namespace ITB.Application.Commands;

public class CriarFabricanteCommand : ICommand
{
    public string nome { get; set;} = string.Empty;
    public string cnpj { get; set;} = string.Empty;
}
