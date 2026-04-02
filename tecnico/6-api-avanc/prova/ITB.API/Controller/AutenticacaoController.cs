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
    [AllowAnonymous] 
    public async Task<IActionResult> Login([FromBody] RealizarLoginCommand command)
    {
        // O retorno agora é tipado como CommandResult
        var resultado = await _handler.Handle(command);

        // 1. Verifica se a operação falhou no Handler
        if (!resultado.Sucesso)
        {
            return Unauthorized(new { mensagem = resultado.Mensagem });
        }

        // 2. Extrai o token da propriedade 'Dados'
        // Fazemos o cast para string pois 'Dados' é do tipo object?
        var token = resultado.Dados?.ToString();

        if (string.IsNullOrEmpty(token))
        {
            return Unauthorized(new { mensagem = "Erro ao gerar o acesso." });
        }

        return Ok(new 
        { 
            Token = token,
            Mensagem = resultado.Mensagem
        });
    }
}