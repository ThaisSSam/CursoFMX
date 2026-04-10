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

    public async Task<IEnumerable<VeiculoListagemDto>> ObterVeiculosOffsetAsync(int numeroPagina, int tamanhoPagina)
    {
        return await _context.Veiculos
            // .OrderByDescending(v => v.Ano)
            // .ThenByDescending(v => v.Id)
            .OrderBy(v => v.Id)
            .Skip(numeroPagina * tamanhoPagina)
            .Take(tamanhoPagina)
            .Select(v => new VeiculoListagemDto
            {
                Id = v.Id,
                Placa = v.Placa,
                Modelo = v.Modelo.Nome,
                Marca = v.Marca.Nome,
                Ano = v.Ano,
                PrecoVenda = v.PrecoVenda
            })
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<VeiculoListagemDto>> ObterVeiculosKeysetAsync(int? ultimoIdDaPagina, int tamanhoPagina)
    {
        return await _context.Veiculos
            // .OrderByDescending(v => v.Ano)
            // .ThenByDescending(v => v.Id)
            .OrderBy(v => v.Id)
            .Where(v => v.Id > ultimoIdDaPagina)
            .Take(tamanhoPagina)
            .Select(v => new VeiculoListagemDto
            {
                Id = v.Id,
                Placa = v.Placa,
                Modelo = v.Modelo.Nome,
                Marca = v.Marca.Nome,
                Ano = v.Ano,
                PrecoVenda = v.PrecoVenda
            })
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<VeiculoExportacaoExcelDto>> ObterComLucro()
    {
        var query = await _context.Veiculos
            .AsNoTracking() // Essencial no CQRS: diz ao EF para não rastrear as entidades na memória, deixando a consulta muito mais rápida
            .Select(v => new VeiculoExportacaoExcelDto
            {
                Id = v.Id,
                Modelo = v.Modelo.Nome,
                Placa = v.Placa,
                Ano = v.Ano,
                Marca = v.Marca.Nome,
                PrecoCusto = v.PrecoCusto,
                PrecoVenda = v.PrecoVenda,
                Lucro = v.PrecoVenda - v.PrecoCusto
            })
            .ToListAsync();

        return query;
        // var query = _context.Database.SqlQuery<VeiculoListagemDto>($@" 
        //     SELECT  
        //         v.id,  
        //         v.placa,  
        //         v.ano, 
        //         m.nome AS modelo,      
        //         mar.nome AS marca,
        //         v.precoVenda
        //     FROM veiculos v 
        //     INNER JOIN modelos m ON v.modelo_id = m.id 
        //     INNER JOIN marcas mar ON m.marca_id = mar.id 
        // ");

        // return await query.ToListAsync();
    }


    public async Task<IEnumerable<VeiculoListagemDto>> ObterTodos()
    {
        var query = await _context.Veiculos
            .AsNoTracking() // Essencial no CQRS: diz ao EF para não rastrear as entidades na memória, deixando a consulta muito mais rápida
            .Select(v => new VeiculoListagemDto
            {
                Id = v.Id,
                Modelo = v.Modelo.Nome,
                Placa = v.Placa,
                Ano = v.Ano,
                Marca = v.Marca.Nome, // O EF Core faz o INNER JOIN automaticamente aqui!
                PrecoCusto = v.PrecoCusto,
                PrecoVenda = v.PrecoVenda
            })
            .ToListAsync();

        return query;
        // var query = _context.Database.SqlQuery<VeiculoListagemDto>($@" 
        //     SELECT  
        //         v.id,  
        //         v.placa,  
        //         v.ano, 
        //         m.nome AS modelo,      
        //         mar.nome AS marca,
        //         v.precoVenda
        //     FROM veiculos v 
        //     INNER JOIN modelos m ON v.modelo_id = m.id 
        //     INNER JOIN marcas mar ON m.marca_id = mar.id 
        // ");

        // return await query.ToListAsync();
    }

    public async Task<VeiculoListagemDto?> ObterPorId(int id)
    {
        var query = _context.Database.SqlQuery<VeiculoListagemDto>($@" 
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

    public async Task<IEnumerable<RelatorioModeloDto>> RelatorioModeloEstoque()
    {
        var query = _context.Database.SqlQuery<RelatorioModeloDto>($@" 
            SELECT  
                m.nome AS nome_modelo,
                COUNT(v.id) AS quantidade
            FROM veiculos v
            INNER JOIN modelos m ON v.modelo_id = m.id
            GROUP BY v.modelo_id, m.nome
        ");

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<DashboardVeiculoDTO>> RelatorioUltimosVeiculosRegistrados(int qtdMaxima)
    {
        return await _context.Veiculos
            .OrderByDescending(v => v.Ano)
            .ThenByDescending(v => v.Id)
            .Select(v => new DashboardVeiculoDTO
            {
                NomeCompleto = $"{v.Marca.Nome} - {v.Placa}",
                Ano = v.Ano
            })
            .Take(qtdMaxima)
            .AsNoTracking()
            .ToListAsync();
    }
}