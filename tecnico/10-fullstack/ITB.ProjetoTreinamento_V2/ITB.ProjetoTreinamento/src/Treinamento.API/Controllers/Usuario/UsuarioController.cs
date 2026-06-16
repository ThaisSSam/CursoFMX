using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Treinamento.Domain.Aggregates.Usuarios;
using Treinamento.Domain.Core.Interfaces;
using Treinamento.Domain.Handlers;
using Treinamento.Infrastructure.Persistence;
using Treinamento.CrossCutting.Dtos;

namespace Treinamento.API.Controllers;

[ApiController]
[Route("usuarios")]
public class UsuarioController : ControllerBase
{
    private readonly ILogarUsuarioHandler _handler;
    private readonly TreinamentoReadContext _readContext;
    public UsuarioController(ILogarUsuarioHandler handler, TreinamentoReadContext readContext)
    {
        _handler = handler;
        _readContext = readContext;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var loginDto = new LoginDto
        {
            Email = request.Email,
            Senha = request.Senha,
            LembrarAcesso = request.LembrarAcesso
        };

        var (sucesso, mensagemErro, token) = await _handler.ExecutarAsync(loginDto);

        if (!sucesso)
        {
            if (mensagemErro.Contains("Limite de tentativas") || mensagemErro.Contains("bloqueado"))
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { errors = new[] { mensagemErro } });
            }

            return Unauthorized(new { errors = new[] { mensagemErro } });
        }

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Expires = request.LembrarAcesso ? DateTime.UtcNow.AddDays(30) : null
        };

        Response.Cookies.Append("X-Access-Token", token!, cookieOptions);

        return Ok(new
        {
            message = "Login realizado com sucesso!",
            token = token
        });
    }

    [Authorize]
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("X-Access-Token");

        return Ok(new { message = "Sessão encerrada com sucesso!" });
    }

    [Authorize]
    [HttpGet] 
    public async Task<IActionResult> ObterTodos()
    {
        try
        {
            var usuarios = await _readContext.Usuarios
                .Select(u => new 
                {
                    u.Id,
                    u.Email,
                    u.Ativo,
                    u.BloqueadoAte
                })
                .ToListAsync();

            return Ok(usuarios);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { errors = new[] { "Erro ao listar usuários: " + ex.Message } });
        }
    }
}