using System;
using ITB.Application.Dtos;
using ITB.Application.Interfaces;
using ITB.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ITB.Infrastructure.Queries;

public class VeiculoQuery : IVeiculoQuery
{
    private readonly AppDbContext _context;

    public VeiculoQuery(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<VeiculosListagemDTO>> ObterTodosAsync()
    {
        // SQL Otimizado: Traz apenas colunas necessárias e já faz os JOINs 
        // O EF Core 8 mapeia o resultado para o DTO baseado nos nomes das colunas (Alias) 

        var query = _context.Database.SqlQuery<VeiculosListagemDTO>($@" 
                SELECT  
                    v.id,  
                    v.placa,  
                    v.ano, 
                    m.nome AS modelo,      
                    mar.nome AS marca      
                FROM veiculos v 
                INNER JOIN modelos m ON v.modelo_id = m.id 
                INNER JOIN marcas mar ON m.marca_id = mar.id 
            ");
        return await query.ToListAsync();
    }
    public async Task<VeiculosListagemDTO?> ObterPorIdAsync(int id)
    {
        var query = _context.Database.SqlQuery<VeiculosListagemDTO>($@" 
                SELECT  
                    v.id,  
                    v.placa,  
                    v.ano, 
                    m.nome AS modelo,      
                    mar.nome AS marca      
                FROM veiculos v 
                INNER JOIN modelos m ON v.modelo_id = m.id 
                INNER JOIN marcas mar ON m.marca_id = mar.id 
                    WHERE v.id = {id} 
            ");
        return await query.FirstOrDefaultAsync();
    }
}
