using System;
using ITB.Application.Commands;
using ITB.Domain.Core.Messages;
using ITB.Domain.Entities;
using ITB.Domain.Interfaces;

namespace ITB.Application.Handlers;

public class CriarFabricanteHandler : IHandler<CriarFabricanteCommand>
{
    private readonly IFabricanteRepository _repository;

    public CriarFabricanteHandler(IFabricanteRepository repository) => _repository = repository;

    public async Task Handle(CriarFabricanteCommand comando)
    {
        var fabricante = new Fabricante
        {
            Nome = comando.Nome,
            Cnpj = comando.Cnpj,
        };
        await _repository.AdicionarAsync(fabricante);
    }
}
