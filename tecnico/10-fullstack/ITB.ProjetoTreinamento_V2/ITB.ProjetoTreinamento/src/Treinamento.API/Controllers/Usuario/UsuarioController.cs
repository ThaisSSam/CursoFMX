using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Treinamento.Application.Services;

namespace Treinamento.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly AutenticacaoService _autenticacaoService;

    public UsuarioController(AutenticacaoService autenticacaoService)
    {
        _autenticacaoService = autenticacaoService;
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        // Se as anotações do DTO (Required, EmailAddress) falharem, o .NET já barra aqui
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var (sucesso, mensagemErro) = await _autenticacaoService.LogarAsync(request);

        if (!sucesso)
        {
            // Trata mensagens de limite excedido ou bloqueio de administrador com códigos específicos se preferir
            if (mensagemErro.Contains("Limite de tentativas") || mensagemErro.Contains("bloqueado"))
            {
                // Devolve no formato que o catch do seu Front-end lê: error.response.data.errors[0]
                return StatusCode(StatusCodes.Status403Forbidden, new { errors = new[] { mensagemErro } });
            }

            // Credenciais inválidas padrão (RN004)
            return Unauthorized(new { errors = new[] { mensagemErro } });
        }

        // 3.a. Credenciais válidas e usuário ativo -> Retorna Sucesso (e os dados do Token/Sessão)
        return Ok(new { message = "Login realizado com sucesso!" });
    }
}