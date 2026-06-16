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
using Treinamento.Domain.Aggregates.Tarefa;
using Treinamento.Infrastructure.Queries;

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
    private readonly TarefaQueryService _queryService; 

    public TarefaController(
        TreinamentoReadContext readContext,
        ITarefaRepository tarefaRepository,
        CriarTarefaHandler criarHandler,
        EditarTarefaHandler editarHandler,
        ExcluirTarefaHandler excluirHandler,
        TarefaQueryService queryService) 
    {
        _readContext = readContext;
        _tarefaRepository = tarefaRepository;
        _criarHandler = criarHandler;
        _editarHandler = editarHandler;
        _excluirHandler = excluirHandler;
        _queryService = queryService;
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
                .IgnoreQueryFilters() 
                .Include(t => t.UsuarioResponsavel)
                .Select(t => new
                {
                    Codigo = t.Id,
                    t.Nome,
                    Situacao= t.Situacao.ToString(),
                    Prioridade = t.Prioridade.ToString(),
                    t.DataCriacao,
                    t.Excluido, 
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
            command.Id = id;
            var resultado = await _editarHandler.ExecutarAsync(command);

            return Ok(new { success = resultado, message = "Tarefa updated com sucesso!" });
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
            var (dados, totalCount) = await _queryService.ExecutarConsultaDinamicaAsync(payload);
            
            return Ok(new { data = dados, totalCount });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { errors = new[] { "Erro ao processar consulta dinâmica: " + ex.Message } });
        }
    }

    // HISTORICO
    [Authorize]
    [HttpGet("{id:int}/historico")]
    public async Task<IActionResult> ObterHistoricoPorTarefa(int id)
    {
        try
        {
            var historico = await _readContext.Set<TarefaHistorico>()
                .Where(h => h.TarefaId == id)
                .OrderByDescending(h => h.DataAlteracao)
                .Select(h => new
                {
                    h.Id,
                    h.TarefaId,
                    h.Nome,
                    Situacao = ((Treinamento.Domain.Aggregates.Tarefa.TipoSituacao)h.Situacao).ToString(),
                    Prioridade = ((Treinamento.Domain.Aggregates.Tarefa.TipoPrioridade)h.Prioridade).ToString(),
                    h.UsuarioId,
                    h.DataAlteracao,
                    h.TipoAcao,
                    h.UsuarioAlteracaoId
                })
                .ToListAsync();

            return Ok(historico);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { errors = new[] { "Erro ao buscar histórico da tarefa: " + ex.Message } });
        }
    }

    [Authorize]
    [HttpGet("historico-geral")]
    public async Task<IActionResult> ObterHistoricoGeral([FromQuery] int pagina = 1, [FromQuery] int registrosPorPagina = 20)
    {
        try
        {
            var query = _readContext.Set<TarefaHistorico>().AsQueryable();

            var totalCount = await query.CountAsync();

            var historicoGeral = await query
                .OrderByDescending(h => h.DataAlteracao)
                .Skip((pagina - 1) * registrosPorPagina)
                .Take(registrosPorPagina)
                .Select(h => new
                {
                    h.Id,
                    h.TarefaId,
                    h.Nome,
                    Situacao = ((Treinamento.Domain.Aggregates.Tarefa.TipoSituacao)h.Situacao).ToString(),
                    Prioridade = ((Treinamento.Domain.Aggregates.Tarefa.TipoPrioridade)h.Prioridade).ToString(),
                    h.UsuarioId,
                    h.DataAlteracao,
                    h.TipoAcao,
                    h.UsuarioAlteracaoId
                })
                .ToListAsync();

            return Ok(new { data = historicoGeral, totalCount });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { errors = new[] { "Erro ao buscar histórico geral: " + ex.Message } });
        }
    }
}