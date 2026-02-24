using System;
using ITB.Domain.Entities;
using ITB.Domain.Interfaces;
using ITB.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ITB.Infrastructure.Repositories;

public class VeiculoRepository :IVeiculoRepository
{
    private readonly AppDbContext _context;

    public VeiculoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AdicionarAsync(Veiculo novoVeiculo)
    {
        await _context.veiculos.AddAsync(novoVeiculo);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Veiculo>?> ObterTodosAsync()
    {
        return await _context.veiculos.ToListAsync();
    }

    public async Task<Veiculo?>ObterPorIdAsync(int id)
    {
        return await _context.veiculos.FindAsync(id);
    }
}
