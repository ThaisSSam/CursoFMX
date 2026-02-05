using System;
using ITB.Domain.Core.Messages;

namespace ITB.Application.Commands;

public class CriarFabricanteCommand : ICommand
{
    public string Nome { get; set;} = string.Empty;
    public string Cnpj { get; set;} = string.Empty;
}
