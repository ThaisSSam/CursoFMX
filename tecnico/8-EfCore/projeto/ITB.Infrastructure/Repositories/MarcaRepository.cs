using ITB.Domain.Entities;
using ITB.Domain.Interfaces;
using ITB.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ITB.Infrastructure.Repositories;

public class MarcaRepository : RepositoryBase<Marca>, IMarcaRepository
{
    private readonly AppDbContext _context;

    public MarcaRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<bool> VerificarExistencia(int id)
    {
        return await _context.Marcas.AnyAsync(m => m.Id == id);
    }

    public async Task<bool> PossuiVeiculosAtivos(int marcaId)
    {
        return await _context.Veiculos
            .AnyAsync(v => v.MarcaId == marcaId && v.Ativo);
    }

    // public async Task AdicionarAsync(Marca novaMarca)
    // {
    //     await _context.Marcas.AddAsync(novaMarca);
    // }

    // public Task AtualizarAsync(Marca marcaAtualizada)
    // {
    //     return Task.CompletedTask;
    // }

    public async Task<List<Marca>?> ObterTodosAsync()
    {
        return await _context.Marcas
            .Include(m => m.Veiculos)
            .ToListAsync();
    }

    public async Task<Marca?> ObterPorIdAsync(int id)
    {
        return await _context.Marcas
            .Include(m => m.Veiculos)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    // public async Task<bool> RemoverAsync(int id)
    // {
    //     var linhasAfetadas = await _context.Marcas
    //         .Where(c => c.Id == id)
    //         .ExecuteDeleteAsync();

    //     return linhasAfetadas > 0;
    // }
}
