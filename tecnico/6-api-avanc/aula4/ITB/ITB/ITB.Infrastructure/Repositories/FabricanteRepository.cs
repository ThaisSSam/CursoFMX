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
        await _context.Fabricantes.AddAsync(novoFabricante);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(Fabricante fabricanteAtualizado)
    {
        _context.Fabricantes.Update(fabricanteAtualizado);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Fabricante>?> ObterTodosAsync()
    {
        return await _context.Fabricantes.ToListAsync();
    }

    public async Task<Fabricante?> ObterPorIdAsync(int id)
    {
        return await _context.Fabricantes.FindAsync(id);
    }

    public async Task<bool> RemoverAsync(int id)
    {
        // Executa o DELETE direto no PostgreSQL em uma única linha
        // Retorna a quantidade de linhas afetadas (0 se não existia, 1 se deletou)
        var linhasAfetadas = await _context.Fabricantes
            .Where(f => f.Id == id)
            .ExecuteDeleteAsync();

        return linhasAfetadas > 0;
    }
}
