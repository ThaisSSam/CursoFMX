using System;
using System.Linq; // Necessário para Select, SelectMany e ToList
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore; 
using ITB.Application.Dtos;
using ITB.Application.Interfaces;
using ITB.Domain.Entities;
using ITB.Infrastructure.Persistence;
using ITB.Infrastructure.Queries;

public class MarcaQuery : IMarcaQuery
{
    private readonly AppDbContext _context;

    public MarcaQuery(AppDbContext context)
    {
        _context = context;
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

      var resultadoHierarquico = dadosPlanos .GroupBy(x => x.MarcaId).Select(grupo => new MarcaComVeiculosDTO {
        Id = grupo.Key,
        Nome = grupo.First().MarcaNome, 
        Ativo = grupo.First().MarcaAtivo,
        
        Veiculos = grupo.Where(x => x.VeiculoId.HasValue).Select(x => new VeiculoSimplesDTO
        {
          Id = x.VeiculoId!.Value,
          Placa = x.VeiculoPlaca!,
          Modelo = x.ModeloNome!,
          Ano = x.VeiculoAno!.Value
        }).ToList()
      });
    return resultadoHierarquico;
  }
  public async Task<IEnumerable<MarcaComVeiculosDTO>> ObterMarcasComVeiculosProjecao()
  {
      return await _context.marcas
          .Select(m => new MarcaComVeiculosDTO
          {
              Id = m.Id,
              Nome = m.Nome,
              Ativo = m.Ativo,
                            
              Veiculos = m.Modelos.SelectMany(mod => mod.Veiculos.Select(v => new VeiculoSimplesDTO
              {
                  Id = v.Id,
                  Placa = v.Placa,
                  Ano = v.Ano,
                  Modelo = mod.Nome 
              })).ToList()
          })
          .ToListAsync();
  }

}