using System;
using ITB.Domain.Entities;

namespace ITB.Domain.Interfaces;

public interface IMarcaRepository : IRepositoryBase<Marca>
{
    // Task<List<Marca>?>ObterTodosAsync();

    // Task<Marca?> ObterPorIdAsync(int id);

    // Task AdicionarAsync(Marca novaMarca);

    Task<bool> VerificarExistencia(int id);
    Task<bool> PossuiModelosAtivos(int marcaId);
}
