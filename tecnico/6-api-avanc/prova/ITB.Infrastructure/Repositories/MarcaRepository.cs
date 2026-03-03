using System;
using ITB.Domain.Entities;
using ITB.Domain.Interfaces;
using ITB.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ITB.Infrastructure.Repositories;

public class MarcaRepository : RepositoryBase<Marca>,IMarcaRepository
{
    private readonly AppDbContext _context;

    public MarcaRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    // public async Task AdicionarAsync(Marca novaMarca)
    // {
    //     await _context.marcas.AddAsync(novaMarca);
    //     await _context.SaveChangesAsync();
    // }

    // public async Task<List<Marca>?> ObterTodosAsync()
    // {
    //     return await _context.marcas.ToListAsync();
    // }

    // public async Task<Marca?> ObterPorIdAsync(int id)
    // {
    //     return await _context.marcas.FindAsync(id);
    // }
    public async Task<bool> VerificarExistencia(int id) =>
        await _context.marcas.AnyAsync(m => m.Id == id);
    public async Task<bool> PossuiModelosAtivos(int marcaId)
    {
        return await _context.veiculos
            .AnyAsync(v => v.ModeloId == marcaId && v.Ativo);
    }

}
