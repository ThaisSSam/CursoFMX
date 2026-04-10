using ITB.Domain.Entities;
using ITB.Domain.Interfaces;
using ITB.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ITB.Infrastructure.Repositories;

public class ModeloRepository : RepositoryBase<Modelo>, IModeloRepository
{
    private readonly AppDbContext _context;

    public ModeloRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    // public async Task AdicionarAsync(Modelo novoModelo)
    // {
    //     await _context.Modelos.AddAsync(novoModelo);
    // }

    // public Task AtualizarAsync(Modelo modeloAtualizado)
    // {
    //     return Task.CompletedTask;
    // }

    public async Task<IEnumerable<Modelo>?> ObterTodosAsync()
    {
        return await _context.Modelos
            .Include(m => m.Marca)
            .ToListAsync();
    }

    public async Task<Modelo?> ObterPorIdAsync(int id)
    {
        return await _context.Modelos
            .Include(m => m.Marca)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    // public async Task<bool> RemoverAsync(int id)
    // {
    //     var linhasAfetadas = await _context.Modelos
    //         .Where(c => c.Id == id)
    //         .ExecuteDeleteAsync();

    //     return linhasAfetadas > 0;
    // }
}
