using System;
using ITB.Application.Commands;
using ITB.Domain.Core.Messages;
using ITB.Domain.Core.Messages.Interfaces;

namespace ITB.Application.Handlers;

public class LogFabricanteHandler: IHandler<CriarFabricanteCommand>
{
    public Task Handle (CriarFabricanteCommand comando)
    {
        Console.WriteLine ($"Fabricante: {comando.nome}");
        return Task.CompletedTask;
    }
}
