using ITB.Application.Commands;
using ITB.API.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ITB.Domain.Core.Messages.Interfaces;
using ITB.Application.Handlers; // Namespace do seu barramento atual

namespace ITB.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AutenticacaoController : ControllerBase
{
    private readonly RealizarLoginHandler _handler;

    public AutenticacaoController(RealizarLoginHandler handler)
    {
        _handler = handler;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] RealizarLoginCommand comando)
    {
        // 1. Use o _handler que você injetou no construtor!
        // O método agora se chama Handle e não EnviarComando
        await _handler.Handle(comando);

        // 2. Verifica se o Token foi injetado no comando pelo Handler
        if (string.IsNullOrEmpty(comando.TokenGerado))
        {
            return Unauthorized(new { mensagem = "Usuário ou senha incorretos." });
        }

        // 3. Retorna o token para o React
        return Ok(new { token = comando.TokenGerado });
    }
}