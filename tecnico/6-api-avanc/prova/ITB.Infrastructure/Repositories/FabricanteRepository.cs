using System;
using ITB.Domain.Entities;
using ITB.Domain.Interfaces;
using ITB.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ITB.Infrastructure.Repositories;

public class FabricanteRepository : IFabricanteRepository
{
    private readonly AppDbContext _context;

    public FabricanteRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task AdicionarAsync(Fabricante novoFabricante)
    {
        await _context.fabricantes.AddAsync(novoFabricante);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(Fabricante fabricanteAtualizado)
    {
        _context.fabricantes.Update(fabricanteAtualizado);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Fabricante>?> ObterTodosAsync()
    {
        return await _context.fabricantes.ToListAsync();
    }

    public async Task<Fabricante?> ObterPorIdAsync(int id)
    {
        return await _context.fabricantes.FindAsync(id);
    }

    public async Task<bool> RemoverAsync(int id)
    {
        var fabricante = await _context.fabricantes.FindAsync(id);
        if (fabricante == null) return false;

        _context.fabricantes.Remove(fabricante);
        return await _context.SaveChangesAsync() > 0;
    }
}
