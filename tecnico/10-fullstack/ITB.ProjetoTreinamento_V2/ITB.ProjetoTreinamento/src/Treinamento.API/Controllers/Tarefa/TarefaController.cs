using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Treinamento.Infrastructure.Persistence;

namespace Treinamento.API.Controllers;

[ApiController]
[Route("tarefas")]
public class TarefaController : ControllerBase
{
    private readonly TreinamentoReadContext _readContext;

    public TarefaController(TreinamentoReadContext readContext)
    {
        _readContext = readContext;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> ObterTodas()
    {
        try
        {
            var tarefas = await _readContext.Tarefa
                .Include(t => t.UsuarioResponsavel) // Faz o JOIN com a tabela de usuários
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
            var tarefa = await _readContext.Tarefa
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