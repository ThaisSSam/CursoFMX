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
        var query = _context.Database.SqlQuery<MarcaVeiculoFlatDTO>($@"
            SELECT 
                m.id AS ""MarcaId"",
                m.nome AS ""MarcaNome"",
                m.ativo AS ""MarcaAtivo"",
                v.id AS ""VeiculoId"",
                v.placa AS ""VeiculoPlaca"",
                v.ano AS ""VeiculoAno"",
                mod.nome AS ""ModeloNome""
            FROM marcas m
            LEFT JOIN modelos mod ON mod.marca_id = m.id
            LEFT JOIN veiculos v ON v.modelo_id = mod.id
        ");

        var dadosPlanos = await query.ToListAsync();

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

    public async Task<IEnumerable<DezUltimosVeiculosDTO>> DezUltimosComVeiculosAsync()
    {
        var query = _context.Database.SqlQuery<MarcaVeiculoFlatDTO>($@" 
                SELECT 
                    v.id AS ""VeiculoId"", 
                    v.placa AS ""VeiculoPlaca"", 
                    v.ano AS ""VeiculoAno"", 
                    mar.nome AS ""MarcaNome"",
                    mar.id AS ""MarcaId"",
                    mar.ativo AS ""MarcaAtivo"",
                    m.nome AS ""ModeloNome""
                FROM veiculos v 
                INNER JOIN modelos m ON v.modelo_id = m.id 
                INNER JOIN marcas mar ON m.marca_id = mar.id 
                ORDER BY v.ano DESC 
                LIMIT 10
            ");

        var dados = await query.ToListAsync();

        return dados.Select(v => new DezUltimosVeiculosDTO
        {
            NomeCompleto = $"{v.MarcaNome} - {v.VeiculoPlaca}",
            Ano = v.VeiculoAno ?? 0
        }).ToList();
    }

    public async Task<IEnumerable<VeiculoRelatorioDTO>> AplicarDescontoAsync(string nomeMarca)
    {
        var relatorio = await _context.marcas
            .Where(m => m.Nome == nomeMarca)
            // SelectMany "achata"> 1 Marca para N Modelos
            .SelectMany(m => m.Modelos)
            // SelectMany > N Modelos para N Veículos
            .SelectMany(mod => mod.Veiculos)
            .Where(v => v.Ano <= 2020)
            .Select(v => new VeiculoRelatorioDTO
            {
                nomeMarca = v.Modelo.Marca.Nome,
                Modelo = v.Modelo.Nome,
                Ano = v.Ano,
                PrecoAtualizado = v.Preco
            })
            .ToListAsync();

        return relatorio;
    }

    // MISSÃO 1 - PASSO A
    public async Task<PaginacaoOffsetResponse<VeiculoRelatorioDTO>> ObterVeiculosOffsetAsync(int numeroPagina, int tamanhoPagina)
    {
        var query = _context.veiculos.AsNoTracking();

        var totalRegistros = await query.CountAsync();
        var pular = (numeroPagina - 1) * tamanhoPagina;

        var dados = await query
            .OrderBy(v => v.Id) // Importante: Sempre ordene
            .Skip(pular)
            .Take(tamanhoPagina)
            .Select(v => new VeiculoRelatorioDTO 
            { 
                Id = v.Id,
                Placa = v.Placa, 
                PrecoAtualizado = v.Preco 
            })
            .ToListAsync();

        return new PaginacaoOffsetResponse<VeiculoRelatorioDTO>
        {
            Dados = dados,
            PaginaAtual = numeroPagina,
            TotalRegistros = totalRegistros,
            TotalPaginas = (int)Math.Ceiling(totalRegistros / (double)tamanhoPagina)
        };
    }

    // MISSÃO 1 - PASSO B
    public async Task<PaginacaoResponse<VeiculoRelatorioDTO>> ObterVeiculosKeysetAsync(int? ultimoIdDaPagina, int tamanhoPagina)
    {
        var query = _context.veiculos.AsNoTracking();

        // Filtra direto pelo ID, aproveitando o índice primário
        if (ultimoIdDaPagina.HasValue)
        {
            query = query.Where(v => v.Id > ultimoIdDaPagina.Value);
        }

        // Busca 1 registro a mais para saber se "TemMaisPaginas" sem precisar de um Count(*)
        var dados = await query
            .OrderBy(v => v.Id)
            .Take(tamanhoPagina + 1)
            .Select(v => new VeiculoRelatorioDTO
            {
                Id = v.Id,
                Placa = v.Placa,
                PrecoAtualizado = v.Preco
            })
            .ToListAsync();

        var response = new PaginacaoResponse<VeiculoRelatorioDTO>();
        
        response.TemMaisPaginas = dados.Count > tamanhoPagina;
        
        if (response.TemMaisPaginas)
        {
            dados.RemoveAt(dados.Count - 1); // Remove o registro extra
        }

        response.Dados = dados;
        response.ProximoCursor = dados.LastOrDefault()?.Id;

        return response;
    } 
}
