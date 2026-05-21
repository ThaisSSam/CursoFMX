using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Treinamento.Domain.Aggregates.Usuarios;
using Treinamento.Domain.Core.Interfaces;
using Treinamento.Domain.Handlers;

namespace Treinamento.API.Controllers;

[ApiController]
[Route("usuarios")]
public class UsuarioController : ControllerBase
{
    private readonly ILogarUsuarioHandler _handler;
    public UsuarioController(ILogarUsuarioHandler handler)
    {
        _handler = handler;
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

        // Injeta o token diretamente no cabeçalho de resposta do navegador
        Response.Cookies.Append("X-Access-Token", token!, cookieOptions);

        // devolve a mensagem e o token 
        return Ok(new 
        { 
            message = "Login realizado com sucesso!",
            token = token
        });
    }
}