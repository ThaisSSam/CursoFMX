using System;
using ITB.Domain.Entities;
using ITB.Domain.Interfaces;
using ITB.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ITB.Infrastructure.Repositories;

public class ModeloRepository : IModeloRepository
{
    private readonly AppDbContext _context;

    public ModeloRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AdicionarAsync(Modelo modelo)
    {
        await _context.modelos.AddAsync(modelo);
    }

    public async Task<IEnumerable<Modelo>> ObterTodos()
    {
        return await _context.modelos
            .AsNoTracking()
            .Include(v => v.Marca)
            .ToListAsync();
    }

    public async Task<Modelo?> ObterPorIdAsync(int id)
    {
        return await _context.modelos.FindAsync(id);
    }
}
