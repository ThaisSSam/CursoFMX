using System;
using ITB.Application.Dtos;
using ITB.Application.Interfaces;
using ITB.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ITB.Infrastructure.Queries;

public class ModeloQuery : IModeloQuery
{
    private readonly AppDbContext _context;

    public ModeloQuery (AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<RelatorioModeloDTO>> ObterTodosAsync()
    {
        var query = _context.Database.SqlQuery<RelatorioModeloDTO>($@"
            SELECT
                m.id,
                m.nome AS nome_modelo,
                (SELECT COUNT(*) FROM veiculos v WHERE v.modelo_id = m.id) AS quantidade
            FROM modelos m
        ");
        return await query.ToListAsync();
    }

    public async Task<RelatorioModeloDTO?> ObterPorIdAsync(int id)
    {
        var query = _context.Database.SqlQuery<RelatorioModeloDTO>($@"
            SELECT
                m.id,
                m.nome AS nome_modelo,
                (SELECT COUNT(*) FROM veiculos v WHERE v.modelo_id = m.id) AS quantidade
            FROM modelos m
            WHERE m.id = {id}
        ");
        return await query.FirstOrDefaultAsync();
    }
}
