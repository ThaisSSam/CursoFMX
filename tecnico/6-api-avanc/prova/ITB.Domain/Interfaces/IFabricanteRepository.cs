using System;
using ITB.Domain.Entities;

namespace ITB.Domain.Interfaces;

public interface IFabricanteRepository
{
    Task<List<Fabricante>?> ObterTodosAsync();
    Task<Fabricante?> ObterPorIdAsync(int id);
    Task<bool> RemoverAsync(int id);
    Task AtualizarAsync(Fabricante fabricanteAtualizado);
    Task AdicionarAsync(Fabricante novoFabricante);
}
