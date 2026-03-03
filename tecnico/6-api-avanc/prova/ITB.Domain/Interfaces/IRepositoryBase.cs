using System;

namespace ITB.Domain.Interfaces;

public interface IRepositoryBase<TEntity> where TEntity : class
{
    Task<TEntity?> ObterPorIdAsync(int id);

    Task<IEnumerable<TEntity>> ObterTodosAsync();

    Task AdicionarAsync(TEntity entity);

    void Remover(TEntity entity);
}
