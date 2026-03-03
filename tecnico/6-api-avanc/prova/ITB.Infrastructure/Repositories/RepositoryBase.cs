using System;
using ITB.Domain.Interfaces;
using ITB.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ITB.Infrastructure.Repositories;

public abstract class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
{
    protected readonly AppDbContext _context;

    // O DbSet é a representação da tabela em memória
    protected readonly DbSet<TEntity> _dbSet;

    public RepositoryBase(AppDbContext context)
    {
        _context = context;
        // O EF Core descobre qual tabela usar baseado no tipo TEntity
        _dbSet = context.Set<TEntity>();
    }
    public virtual async Task<TEntity?> ObterPorIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }
    public virtual async Task<IEnumerable<TEntity>> ObterTodosAsync()
    {
        return await _dbSet.AsNoTracking().ToListAsync();
    }
    public async Task AdicionarAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
        // Lembre-se: Não damos SaveChanges aqui! O Unit of Work fará isso.
    }
    public void Remover(TEntity entity)
    {
        _dbSet.Remove(entity);
    }
}