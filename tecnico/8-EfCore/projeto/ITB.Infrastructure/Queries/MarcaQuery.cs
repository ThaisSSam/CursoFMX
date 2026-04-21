using ITB.Application.Dtos;
using ITB.Application.Interfaces;
using ITB.Infrastructure.Persistence;
using ITB.Infrastructure.Queries.Dtos;
using Microsoft.EntityFrameworkCore;

namespace ITB.Infrastructure.Queries;

public class MarcaQuery : IMarcaQuery
{
    private readonly AppDbContext _context;

    public MarcaQuery(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MarcaComVeiculoDto>> ObterMarcasComVeiculosProjecao()
    {
        return await _context.Marcas
            .Select(m => new MarcaComVeiculoDto
            {
                Id = m.Id,
                Nome = m.Nome,
                Ativo = m.Ativo,
                Veiculos = m.Veiculos.Select(v => new VeiculoSimplesDto
                {
                    Id = v.Id,
                    Placa = v.Placa,
                    Ano = v.Ano,
                    Modelo = v.Modelo.Nome
                }).ToList()
            }).ToListAsync();
    }

    public async Task<IEnumerable<MarcaComVeiculoDto>> ObterMarcasComVeiculos()
    {
        var query = _context.Database.SqlQuery<MarcaVeiculoFlatDto>($@"
            SELECT
                m.id AS marca_id,
                m.nome AS marca_nome,
                m.ativo AS marca_ativo,
                v.id AS veiculo_id,
                v.placa AS veiculo_placa,
                v.ano AS veiculo_ano,
                mod.nome AS modelo_nome
            FROM marcas m
            LEFT JOIN modelos mod ON mod.marca_id = m.id
            LEFT JOIN veiculos v ON v.modelo_id = mod.id
        ");

        var dadosPlanos = await query.ToListAsync();

        var resultadoHierarquico = dadosPlanos
            .GroupBy(x => x.MarcaId)
            .Select(grupo => new MarcaComVeiculoDto
            {
                Id = grupo.Key,
                Nome = grupo.First().MarcaNome,
                Ativo = grupo.First().MarcaAtivo,
                Veiculos = grupo
                    .Where(x => x.VeiculoId.HasValue)
                    .Select(x => new VeiculoSimplesDto
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
