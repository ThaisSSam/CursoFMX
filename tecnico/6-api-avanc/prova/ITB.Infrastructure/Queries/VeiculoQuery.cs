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

    public async Task<IEnumerable<MarcaComVeiculosDTO>> ObterMarcasComVeiculosAsync()
    {
        // SQL Flat para buscar os dados desnormalizados
        var query = _context.Database.SqlQuery<MarcaVeiculoFlatDTO>($@"
            SELECT 
                m.id AS MarcaId,
                m.nome AS MarcaNome,
                m.ativo AS MarcaAtivo,
                v.id AS VeiculoId,
                v.placa AS VeiculoPlaca,
                v.ano AS VeiculoAno,
                mod.nome AS ModeloNome
            FROM marcas m
            LEFT JOIN modelos mod ON mod.marca_id = m.id
            LEFT JOIN veiculos v ON v.modelo_id = mod.id
        ");

        var dadosPlanos = await query.ToListAsync();

        // Agrupamento em memória para montar o objeto hierárquico (Marca -> List<Veiculo>)
        var resultadoHierarquico = dadosPlanos
            .GroupBy(x => x.MarcaId)
            .Select(grupo => new MarcaComVeiculosDTO
            {
                Id = grupo.Key,
                Nome = grupo.First().MarcaNome,
                Ativo = grupo.First().MarcaAtivo,
                Veiculos = grupo
                    .Where(x => x.VeiculoId.HasValue)
                    .Select(x => new VeiculoSimplesDTO
                    {
                        Id = x.VeiculoId!.Value,
                        Placa = x.VeiculoPlaca!,
                        Modelo = x.ModeloNome!,
                        Ano = x.VeiculoAno!.Value
                    }).ToList()
            });

        return resultadoHierarquico;
    }
}
