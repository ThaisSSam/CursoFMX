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

      var resultadoHierarquico = dadosPlanos .GroupBy(x => x.MarcaId).Select(grupo => new MarcaComVeiculosDTO {
        Id = grupo.Key,
        // Pegamos os dados do primeiro item do grupo (dados da marca são repetidos)
        Nome = grupo.First().MarcaNome, 
        Ativo = grupo.First().MarcaAtivo,
        
        // 4. Projetamos a lista de filhos
        // Filtramos onde VeiculoId != null (ignora marcas sem carros)
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
              
              // Aqui pegamos todos os modelos da marca e "achatamos" seus veículos
              Veiculos = m.Modelos.SelectMany(mod => mod.Veiculos.Select(v => new VeiculoSimplesDTO
              {
                  Id = v.Id,
                  Placa = v.Placa,
                  Ano = v.Ano,
                  Modelo = mod.Nome // O nome vem do modelo pai na iteração
              })).ToList()
          })
          .ToListAsync();
  }

}