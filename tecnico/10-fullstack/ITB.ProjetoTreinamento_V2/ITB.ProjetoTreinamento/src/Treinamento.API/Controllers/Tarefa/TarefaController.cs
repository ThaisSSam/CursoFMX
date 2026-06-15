using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Treinamento.Domain.Commands;
using Treinamento.Domain.Handlers;
using Treinamento.Infrastructure.Persistence;
using Treinamento.Domain.Aggregates.Tarefa.Interfaces;

namespace Treinamento.API.Controllers.Tarefa;

[ApiController]
[Route("tarefas")]
public class TarefaController : ControllerBase
{
    private readonly TreinamentoReadContext _readContext;
    private readonly CriarTarefaHandler _criarHandler;
    private readonly ITarefaRepository _tarefaRepository;
    private readonly EditarTarefaHandler _editarHandler;
    private readonly ExcluirTarefaHandler _excluirHandler;

    public TarefaController(
        TreinamentoReadContext readContext,
        ITarefaRepository tarefaRepository,
        CriarTarefaHandler criarHandler,
        EditarTarefaHandler editarHandler,
        ExcluirTarefaHandler excluirHandler)
    {
        _readContext = readContext;
        _tarefaRepository = tarefaRepository;
        _criarHandler = criarHandler;
        _editarHandler = editarHandler;
        _excluirHandler = excluirHandler;
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarTarefaCommand command)
    {
        var resultado = await _criarHandler.ExecutarAsync(command);
        return Ok(new { success = resultado, message = "Tarefa criada com sucesso!" });
    }

    [Authorize]
    [HttpGet("dashboard-cards")]
    public async Task<IActionResult> ObterCardsDashboard()
    {
        try
        {
            var dataLimiteAtraso = DateTime.UtcNow.AddDays(-5);
            var progressoResponsaveis = await _readContext.Tarefas
                .Include(t => t.UsuarioResponsavel)
                .GroupBy(t => new
                {
                    t.UsuarioId,
                    Nome = t.UsuarioResponsavel != null ? t.UsuarioResponsavel.Nome : "Sem dono"
                })
                .Select(g => new
                {
                    Nome = g.Key.Nome,
                    TotalTarefas = g.Count(),
                    EmAberto = g.Count(t => (int)t.Situacao == 1),
                    EmAndamento = g.Count(t => (int)t.Situacao == 2),
                    Concluidas = g.Count(t => (int)t.Situacao == 3),
                    Atrasadas = g.Count(t => (int)t.Situacao != 3 && t.DataCriacao < dataLimiteAtraso),
                    PrioridadeAlta = g.Count(t => (int)t.Prioridade == 3),
                    PrioridadeMedia = g.Count(t => (int)t.Prioridade == 2),
                    PrioridadeBaixa = g.Count(t => (int)t.Prioridade == 1)
                })
                .ToListAsync();

            return Ok(progressoResponsaveis);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { errors = new[] { "Erro ao gerar métricas do dashboard: " + ex.Message } });
        }
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> ObterTodas()
    {
        try
        {
            var tarefas = await _readContext.Tarefas
                .Include(t => t.UsuarioResponsavel)
                .Select(t => new
                {
                    Codigo = t.Id,
                    t.Nome,
                    t.Situacao,
                    t.Prioridade,
                    t.DataCriacao,
                    Responsavel = t.UsuarioResponsavel != null ? new
                    {
                        Id = t.UsuarioResponsavel.Id,
                        Email = t.UsuarioResponsavel.Email,
                        Nome = t.UsuarioResponsavel.Nome,
                    } : null
                })
                .ToListAsync();

            return Ok(tarefas);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { errors = new[] { "Erro ao listar tarefas: " + ex.Message } });
        }
    }

    [Authorize]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObterPorId(int id)
    {
        try
        {
            var tarefa = await _readContext.Tarefas
                .Include(t => t.UsuarioResponsavel)
                .Where(t => t.Id == id)
                .Select(t => new
                {
                    Codigo = t.Id,
                    t.Nome,
                    t.Situacao,
                    t.Prioridade,
                    t.DataCriacao,
                    Responsavel = t.UsuarioResponsavel != null ? new
                    {
                        Id = t.UsuarioResponsavel.Id,
                        Email = t.UsuarioResponsavel.Email
                    } : null
                })
                .FirstOrDefaultAsync();

            if (tarefa == null)
            {
                return NotFound(new { errors = new[] { "Tarefa não encontrada." } });
            }

            return Ok(tarefa);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { errors = new[] { "Erro ao buscar tarefa: " + ex.Message } });
        }
    }

    [Authorize]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Atualizar(int id, [FromBody] EditarTarefaCommand command)
    {
        try
        {
            var commandComId = command with { Id = id };
            var resultado = await _editarHandler.ExecutarAsync(commandComId);

            return Ok(new { success = resultado, message = "Tarefa atualizada com sucesso!" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { errors = new[] { "Erro ao atualizar tarefa: " + ex.Message } });
        }
    }

    [Authorize]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Excluir(int id)
    {
        try
        {
            var command = new ExcluirTarefaCommand(id);
            var resultado = await _excluirHandler.ExecutarAsync(command);

            return Ok(new { success = resultado, message = "Tarefa excluída com sucesso!" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { errors = new[] { "Erro ao excluir tarefa: " + ex.Message } });
        }
    }

    [HttpGet("situacoes")]
    public IActionResult ObterSituacoes()
    {
        var situacoes = Enum.GetValues(typeof(Treinamento.Domain.Aggregates.Tarefa.TipoSituacao))
            .Cast<Treinamento.Domain.Aggregates.Tarefa.TipoSituacao>()
            .Select(s => new { id = (int)s, label = s.ToString() });

        return Ok(situacoes);
    }

    [HttpGet("prioridades")]
    public IActionResult ObterPrioridades()
    {
        var prioridades = Enum.GetValues(typeof(Treinamento.Domain.Aggregates.Tarefa.TipoPrioridade))
            .Cast<Treinamento.Domain.Aggregates.Tarefa.TipoPrioridade>()
            .Select(p => new { id = (int)p, label = p.ToString() });

        return Ok(prioridades);
    }

    [Authorize]
    [HttpPost("consultar")]
    public async Task<IActionResult> Consultar([FromBody] JsonElement payload)
    {
        try
        {
            var tarefasQuery = _readContext.Tarefas.Include(t => t.UsuarioResponsavel).AsQueryable();

            if (payload.TryGetProperty("grupoRaiz", out var grupoRaiz) &&
                grupoRaiz.TryGetProperty("filtros", out var filtros))
            {
                foreach (var filtro in filtros.EnumerateArray())
                {
                    string nomeParametro = filtro.GetProperty("nomeParametro").GetString();
                    var valores = filtro.GetProperty("valores");

                    if (valores.ValueKind == JsonValueKind.Array && valores.GetArrayLength() > 0)
                    {
                        if (nomeParametro == "situacao")
                        {
                            var situacoesIds = valores.EnumerateArray().Select(v => v.GetInt32()).ToList();
                            tarefasQuery = tarefasQuery.Where(t => situacoesIds.Contains((int)t.Situacao));
                        }

                        else if (nomeParametro == "prioridade")
                        {
                            var prioridadesIds = valores.EnumerateArray().Select(v => v.GetInt32()).ToList();
                            tarefasQuery = tarefasQuery.Where(t => prioridadesIds.Contains((int)t.Prioridade));
                        }

                        else if (nomeParametro == "usuarioId" || nomeParametro == "responsavelBusca")
                        {
                            int idFiltro = valores.EnumerateArray().First().GetInt32();

                            tarefasQuery = tarefasQuery.Where(t => t.UsuarioId.Equals(idFiltro));
                        }

                        else if (nomeParametro == "dataCriacao")
                        {
                            var dataStr = valores.EnumerateArray().First().GetString();
                            if (DateTime.TryParse(dataStr, out var dataMinima))
                            {
                                var dataMinimaUtc = DateTime.SpecifyKind(dataMinima.Date, DateTimeKind.Utc);
                                tarefasQuery = tarefasQuery.Where(t => t.DataCriacao >= dataMinimaUtc);
                            }
                        }
                    }
                }
            }

            if (payload.TryGetProperty("grupoRaiz", out var grupoRaizTexto) &&
                grupoRaizTexto.TryGetProperty("grupos", out var grupos))
            {
                foreach (var grupo in grupos.EnumerateArray())
                {
                    if (grupo.TryGetProperty("filtros", out var filtrosTexto))
                    {
                        foreach (var filtro in filtrosTexto.EnumerateArray())
                        {
                            string nomeParametro = filtro.GetProperty("nomeParametro").GetString();
                            var valores = filtro.GetProperty("valores");

                            if (valores.ValueKind == JsonValueKind.Array && valores.GetArrayLength() > 0)
                            {
                                var termoBusca = valores.EnumerateArray().First().GetString()?.ToLower().Trim();

                                if (!string.IsNullOrEmpty(termoBusca))
                                {
                                    if (nomeParametro == "nome")
                                    {
                                        tarefasQuery = tarefasQuery.Where(t => t.Nome.ToLower().Contains(termoBusca));
                                    }
                                    else if (nomeParametro == "codigo")
                                    {
                                        if (int.TryParse(termoBusca, out var codigoNum))
                                        {
                                            tarefasQuery = tarefasQuery.Where(t => t.Id == codigoNum);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            int pagina = payload.TryGetProperty("pagina", out var p) ? p.GetInt32() : 1;
            int registrosPorPagina = payload.TryGetProperty("registrosPorPagina", out var r) ? r.GetInt32() : 10;

            var totalCount = await tarefasQuery.CountAsync();

            var tarefasLista = await tarefasQuery
                .Skip((pagina - 1) * registrosPorPagina)
                .Take(registrosPorPagina)
                .Select(t => new
                {
                    Codigo = t.Id,
                    t.Nome,
                    t.Situacao,
                    t.Prioridade,
                    t.DataCriacao,
                    Responsavel = t.UsuarioResponsavel != null ? new
                    {
                        Id = t.UsuarioResponsavel.Id,
                        Email = t.UsuarioResponsavel.Email,
                        Nome = t.UsuarioResponsavel.Nome
                    } : null
                })
                .ToListAsync();

            return Ok(new { data = tarefasLista, totalCount = totalCount });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { errors = new[] { "Erro ao processar consulta dinâmica: " + ex.Message } });
        }
    }
}