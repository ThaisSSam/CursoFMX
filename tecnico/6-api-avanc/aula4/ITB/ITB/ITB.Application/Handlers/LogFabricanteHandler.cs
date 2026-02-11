using System;
using ITB.Application.Commands;
using ITB.Domain.Core.Messages;

namespace ITB.Application.Handlers;

public class LogFabricanteHandler: IHandler<CriarFabricanteCommand>
{
    public Task Handle (CriarFabricanteCommand comando)
    {
        Console.WriteLine ($"Fabricante: {comando.Nome}");
        return Task.CompletedTask;
    }
}
