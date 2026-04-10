
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ITB.API.Controllers.Base;
using ITB.Application.Commands;
using ITB.Application.Handlers;
using ITB.Domain.Core.Messages;
using ITB.Domain.Core.Messages.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ITB.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AutenticacaoController(
    RealizarLoginHandler loginHandler, // Injetando o seu Handler atualizado 
    IDomainNotificationHandler<DomainNotification> notifications)
    : BaseController(notifications)
{
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] RealizarLoginCommand command)
    {
        // 1. O Handler executa a busca, a validação de BCrypt e confere a trava 
        var resultado = await loginHandler.Handle(command);

        // 2. Se falhou (email não existe ou senha errada) 
        if (!resultado.IsSuccess)
            return Unauthorized(new { sucesso = false, erros = resultado.Errors });

        var dadosLogin = resultado.Value;

        // 3. O Handler avisou que precisa trocar a senha? 
        if (dadosLogin!.ExigeTrocaDeSenha)
        {
            return StatusCode(403, new
            {
                sucesso = false,
                mensagem = "Você está usando uma senha provisória. É obrigatório alterar a senha antes de acessar o sistema.",
                redirecionarParaTroca = true
            });
        }

        // 4. Caminho feliz! 
        return Ok(new { Token = dadosLogin.Token });
    }
}
