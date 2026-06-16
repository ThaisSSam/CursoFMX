using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Treinamento.Infrastructure.Persistence;

namespace Treinamento.Infrastructure.Queries;

public class TarefaQueryService
{
    private readonly TreinamentoReadContext _readContext;

    public TarefaQueryService(TreinamentoReadContext readContext)
    {
        _readContext = readContext;
    }

    public async Task<(object Dados, int TotalCount)> ExecutarConsultaDinamicaAsync(JsonElement payload)
    {
        var tarefasQuery = _readContext.Tarefas
            .IgnoreQueryFilters()
            .Include(t => t.UsuarioResponsavel)
            .AsQueryable();

        if (payload.TryGetProperty("grupoRaiz", out var grupoRaiz) && grupoRaiz.TryGetProperty("filtros", out var filtros))
        {
            foreach (var filtro in filtros.EnumerateArray())
            {
                string nomeParametro = filtro.GetProperty("nomeParametro").GetString()?.ToLower().Trim();
                var valores = filtro.GetProperty("valores");

                if (valores.ValueKind != JsonValueKind.Array || valores.GetArrayLength() == 0) continue;

                switch (nomeParametro)
                {
                    case "situacao":
                        var situacoesIds = valores.EnumerateArray().Select(v => v.GetInt32()).ToList();
                        tarefasQuery = tarefasQuery.Where(t => situacoesIds.Contains((int)t.Situacao));
                        break;

                    case "prioridade":
                        var prioridadesIds = valores.EnumerateArray().Select(v => v.GetInt32()).ToList();
                        tarefasQuery = tarefasQuery.Where(t => prioridadesIds.Contains((int)t.Prioridade));
                        break;

                    case "usuarioid":
                    case "responsavelbusca":
                        int idFiltro = valores.EnumerateArray().First().GetInt32();
                        tarefasQuery = tarefasQuery.Where(t => t.UsuarioId == idFiltro);
                        break;

                    case "datacriacao":
                        var dataStr = valores.EnumerateArray().First().GetString();
                        if (DateTime.TryParse(dataStr, out var dataMinima))
                        {
                            var dataMinimaUtc = DateTime.SpecifyKind(dataMinima.Date, DateTimeKind.Utc);
                            tarefasQuery = tarefasQuery.Where(t => t.DataCriacao >= dataMinimaUtc);
                        }
                        break;
                }
            }
        }

        if (payload.TryGetProperty("grupoRaiz", out var grupoRaizTexto) && grupoRaizTexto.TryGetProperty("grupos", out var grupos))
        {
            foreach (var grupo in grupos.EnumerateArray())
            {
                if (!grupo.TryGetProperty("filtros", out var filtrosTexto)) continue;

                foreach (var filtro in filtrosTexto.EnumerateArray())
                {
                    string nomeParametro = filtro.GetProperty("nomeParametro").GetString()?.ToLower().Trim();
                    var valores = filtro.GetProperty("valores");

                    if (valores.ValueKind != JsonValueKind.Array || valores.GetArrayLength() == 0) continue;

                    var termoBusca = valores.EnumerateArray().First().GetString()?.ToLower().Trim();
                    if (string.IsNullOrEmpty(termoBusca)) continue;

                    switch (nomeParametro)
                    {
                        case "nome":
                            tarefasQuery = tarefasQuery.Where(t => t.Nome.ToLower().Contains(termoBusca));
                            break;

                        case "codigo":
                            if (int.TryParse(termoBusca, out var codigoNum))
                            {
                                tarefasQuery = tarefasQuery.Where(t => t.Id == codigoNum);
                            }
                            break;
                    }
                }
            }
        }

        int pagina = payload.TryGetProperty("pagina", out var p) ? p.GetInt32() : 1;
        int registrosPorPagina = payload.TryGetProperty("registrosPorPagina", out var r) ? r.GetInt32() : 10;

        var totalCount = await tarefasQuery.CountAsync();

        var listaProjetada = await tarefasQuery
            .Skip((pagina - 1) * registrosPorPagina)
            .Take(registrosPorPagina)
            .Select(t => new
            {
                Codigo = t.Id,
                t.Nome,
                Situacao = t.Situacao.ToString(),
                Prioridade = t.Prioridade.ToString(),
                t.DataCriacao,
                t.Excluido,
                Responsavel = t.UsuarioResponsavel != null ? new
                {
                    Id = t.UsuarioResponsavel.Id,
                    Email = t.UsuarioResponsavel.Email,
                    Nome = t.UsuarioResponsavel.Nome
                } : null
            })
            .ToListAsync();

        return (listaProjetada, totalCount);
    }
}