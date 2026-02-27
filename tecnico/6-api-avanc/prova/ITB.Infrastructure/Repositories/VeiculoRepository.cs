using System;
using ITB.Domain.Entities;
using ITB.Domain.Interfaces;
using ITB.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ITB.Infrastructure.Repositories;
public class VeiculoRepository : IVeiculoRepository
{
    private readonly AppDbContext _context;

    public VeiculoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AdicionarAsync(Veiculo veiculo)
    {
        // await _context.veiculos.AddAsync(veiculo);
        // await _context.SaveChangesAsync();
        await _context.veiculos.AddAsync(veiculo); 
    }

    // public async Task Atualizar(Veiculo veiculo)
    // {
    //     _context.veiculos.Update(veiculo);
    //     await _context.SaveChangesAsync(); 
        
    // }
    public Task Atualizar(Veiculo veiculo)
    {         
        return Task.CompletedTask;
    }

    public async Task<IEnumerable<Veiculo>> ObterTodos()
    {
        return await _context.veiculos
            .AsNoTracking()
            .Include(v => v.Modelo)
            .ToListAsync();
    }

    public async Task<Veiculo?> ObterPorId(int id) 
    {
        return await _context.veiculos.FindAsync(id); 
    }

    public async Task<bool> PlacaJaExiste(string placa, int veiculoIdIgnorado)
    {
        return await _context.veiculos
            .IgnoreQueryFilters() 
            .AnyAsync(v => v.Placa == placa && v.Id != veiculoIdIgnorado);
    }
}