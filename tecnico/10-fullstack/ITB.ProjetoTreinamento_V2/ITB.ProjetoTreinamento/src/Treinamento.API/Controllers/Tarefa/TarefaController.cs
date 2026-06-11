using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Treinamento.Domain.Commands;
using Treinamento.Domain.Handlers;
using Treinamento.Infrastructure.Persistence;

namespace Treinamento.API.Controllers.Tarefa;

[ApiController]
[Route("tarefas")]
public class TarefaController : ControllerBase
{
    private readonly TreinamentoReadContext _readContext;
    private readonly CriarTarefaHandler _handler;

    public TarefaController(TreinamentoReadContext readContext, CriarTarefaHandler handler)
    {
        _readContext = readContext;
        _handler = handler;
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarTarefaCommand command)
    {
        var resultado = await _handler.ExecutarAsync(command);
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
}