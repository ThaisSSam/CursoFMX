using ITB.Application.Commands;
using ITB.API.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ITB.Domain.Core.Messages.Interfaces; // Namespace do seu barramento atual

namespace ITB.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AutenticacaoController : ControllerBase
{
    private readonly IMessageBus _bus;

    // Injeção do barramento customizado que você já possui no projeto
    public AutenticacaoController(IMessageBus bus)
    {
        _bus = bus;
    }

    [HttpPost("login")]
    [AllowAnonymous] // Aberto para quem ainda não tem Token
    [ApiKey] // Protegido pela chave do App
    public async Task<IActionResult> Login([FromBody] RealizarLoginCommand command)
    {
        // 1. Envia o comando para o seu Handler (RealizarLoginHandler)
        var resultado = await _bus.EnviarComando(command);

        // 2. Verifica se o login foi bem-sucedido no banco
        if (resultado.Sucesso)
        {
            // 3. Retorna o Token (que o Handler guardou na propriedade Dados)
            return Ok(new { Token = resultado.Dados });
        }

        // Caso contrário, retorna erro de credenciais
        return Unauthorized(new { mensagem = resultado.Mensagem });
    }
}