using System;
using ITB.Domain.Interfaces;

namespace ITB.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Commit()
    {
        // SaveChangesAsync retorna o n° de linhas afetadas no banco
        // Se for maior que 0 o commit funcionou
        return await _context.SaveChangesAsync()> 0;
    }
}
