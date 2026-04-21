using ITB.Domain.Entities;
using ITB.Domain.Interfaces;
using ITB.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ITB.Infrastructure.Repositories;

public class VeiculoRepository : RepositoryBase<Veiculo>, IVeiculoRepository
{
    private readonly AppDbContext _context;

    public VeiculoRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    // public async Task AdicionarAsync(Veiculo novoVeiculo)
    // {
    //     await _context.Veiculos.AddAsync(novoVeiculo);
    // }

    // public Task AtualizarAsync(Veiculo veiculoAtualizado)
    // {
    //     // _context.Veiculos.Update(veiculoAtualizado);
    //     return Task.CompletedTask;
    // }

    public async Task<IEnumerable<Veiculo>?> ObterTodosAsync()
    {
        return await _context.Veiculos
            .Include(v => v.Modelo)
            .Include(v => v.Modelo.Marca)
            .Include(v => v.Marca)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Veiculo?> ObterPorIdAsync(int id)
    {
        return await _context.Veiculos
            .Include(v => v.Modelo)
            .Include(v => v.Modelo.Marca)
            .Include(v => v.Marca)
            // .AsNoTracking()
            .FirstOrDefaultAsync(v => v.Id == id);
    }

    public async Task<int> DescontoEmMassaPorMarca(string marca, decimal percentualDesconto, int ano)
    {
        var linhasAfetadas = await _context.Veiculos
        .Where(v => v.Ano <= ano && v.Marca.Nome == marca)
            .ExecuteUpdateAsync(setters => setters
            .SetProperty(v => v.PrecoCusto, v => v.PrecoCusto - (v.PrecoCusto * (percentualDesconto / 100)))
        );
        return linhasAfetadas;
    }

    // public async Task<bool> RemoverAsync(int id)
    // {
    //     var linhasAfetadas = await _context.Veiculos
    //         .Where(c => c.Id == id)
    //         .ExecuteDeleteAsync();

    //     return linhasAfetadas > 0;
    // }

    public async Task<bool> PlacaJaExiste(string placa, int veiculoIdIgnorado)
    {
        return await _context.Veiculos
            .IgnoreQueryFilters()
            .AnyAsync(v => v.Placa == placa && v.Id != veiculoIdIgnorado);
    }
}
