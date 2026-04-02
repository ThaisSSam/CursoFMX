using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        await _context.veiculos.AddAsync(veiculo);
    }

    public Task Atualizar(Veiculo veiculo)
    {
        _context.veiculos.Update(veiculo);
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

    public async Task AplicarDescontoAsync(string nomeMarca)
    {
        // Ajustado para usar PrecoVenda
        await _context.veiculos
            .Where(v => v.Modelo.Marca.Nome == nomeMarca && v.Ano <= 2020)
            .ExecuteUpdateAsync(s => s
                .SetProperty(v => v.PrecoVenda, v => v.PrecoVenda * 0.95m));
    }
}