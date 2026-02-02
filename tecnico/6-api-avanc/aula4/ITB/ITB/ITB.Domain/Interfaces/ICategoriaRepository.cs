using System;
using ITB.Domain.Entities;

namespace ITB.Domain.Interfaces;

public interface ICategoriaRepository
{
    Task<List<Categoria>?>ObterTodosAsync();

    Task<Categoria?> ObterPorIdAsync(int id);

    Task<bool> RemoverAsync(int id);

    Task AtualizarAsync(Categoria categoriaAtualizada);

    Task AdicionarAsync(Categoria novaCategoria);
}
