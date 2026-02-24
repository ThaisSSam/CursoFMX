using System;
using ITB.Domain.Entities;

namespace ITB.Domain.Interfaces;

public interface IMarcaRepository
{
    Task<List<Marca>?>ObterTodosAsync();

    Task<Marca?> ObterPorIdAsync(int id);

    Task AdicionarAsync(Marca novaMarca);
}
