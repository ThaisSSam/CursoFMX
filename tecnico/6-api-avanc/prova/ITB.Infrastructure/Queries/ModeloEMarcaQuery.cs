using System;
using ITB.Application.Dtos;
using ITB.Application.Interfaces;
using ITB.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ITB.Infrastructure.Queries;
// 6. Aqui você escreve o SQL ou LINQ que realmente busca os dados.dps vem o controller
public class ModeloEMarca : IModeloQuery
{
    private readonly AppDbContext _context;

    public ModeloEMarca(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ModeloDropdownDTO>> ObterModelosParaDropdown()
    {
        return await _context.Modelos
            .AsNoTracking()
            .Where(m => m.Ativo)
            .Select(m => new ModeloDropdownDTO
            {
                Id = m.Id,
                NomeExibicao = $"{m.Marca.Nome} - {m.Nome}"
            })
            .OrderBy(m => m.NomeExibicao)
            .ToListAsync();
    }

    // Implementação obrigatória do ObterTodosAsync
    public async Task<IEnumerable<RelatorioModeloDTO>> ObterTodosAsync()
    {
        return await _context.Modelos
            .AsNoTracking()
            .Select(m => new RelatorioModeloDTO
            {
                Id = m.Id,
                Nome = m.Nome,
                NomeMarca = m.Marca.Nome,
                Ativo = m.Ativo
            })
            .ToListAsync();
    }

    // Implementação obrigatória do ObterPorIdAsync
    public async Task<RelatorioModeloDTO?> ObterPorIdAsync(int id)
    {
        return await _context.Modelos
            .AsNoTracking()
            .Where(m => m.Id == id)
            .Select(m => new RelatorioModeloDTO
            {
                Id = m.Id,
                Nome = m.Nome,
                NomeMarca = m.Marca.Nome,
                Ativo = m.Ativo
            })
            .FirstOrDefaultAsync();
    }
}
