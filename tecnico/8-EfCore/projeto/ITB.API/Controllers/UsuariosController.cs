using System;
using System.Security.Claims;
using ITB.API.Controllers.Base;
using ITB.Application.Commands;
using ITB.Application.Dtos;
using ITB.Domain.Core.Messages;
using ITB.Domain.Core.Messages.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITB.API.Controllers;

[Route("api/[controller]")]
[ApiController]
// Primary Constructor injetando apenas o Bus e o Bloco de Notas!
public class UsuariosController(
    IMessageBus bus,
    IDomainNotificationHandler<DomainNotification> notifications) 
    : BaseController(notifications)
{
    
    /// <summary>
    /// Cadastro de um novo usuário. (Aberto ao público)
    /// </summary>
    [HttpPost("registrar")]
    [AllowAnonymous] 
    public async Task<IActionResult> Registrar([FromBody] AdicionarUsuarioRequest request)
    {
        var command = new AdicionarUsuarioCommand
        {
            Nome = request.Nome,
            Email = request.Email,
            Senha = request.SenhaHash,
            Perfil = request.Perfil
        };

        // Envia para o mensageiro (Ele acha o Handler e executa a persistência)
        await bus.EnviarComando(command);

        // A BaseController verifica os erros. Se deu tudo certo, devolvemos o ID gerado.
        return await Response(new { Id = command.IdGerado, Mensagem = "Usuário criado com sucesso!" });
    }

    /// <summary>
    /// Altera a senha do usuário logado. (Protegido)
    /// </summary>
    [HttpPut("alterar-senha")]
    [Authorize] // Exige que o usuário tenha um Token válido
    public async Task<IActionResult> AlterarSenha([FromBody] AlterarSenhaRequest request)
    {
        //Segurança Máxima: Extraímos o ID direto do Token JWT (ClaimTypes.NameIdentifier)
        // Isso impede que o usuário "A" mande o ID do usuário "B" no JSON e troque a senha dele.
        var usuarioIdLogado = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var command = new AlterarSenhaCommand
        {
            UsuarioId = usuarioIdLogado,
            SenhaAtual = request.SenhaAtual,
            NovaSenha = request.NovaSenha
        };

        await bus.EnviarComando(command);

        return await Response("Senha alterada com sucesso!");
    }

    /// <summary>
    /// Fluxo de "Esqueci minha senha". (Aberto ao público)
    /// </summary>
    [HttpPost("resetar-senha")]
    [AllowAnonymous]
    public async Task<IActionResult> ResetarSenha([FromBody] ResetarSenhaRequest request)
    {
        var command = new ResetarSenhaCommand { Email = request.Email };
        
        await bus.EnviarComando(command);

        // NOTA DE AULA: Estamos devolvendo a senha provisória na tela apenas para fins didáticos.
        // Em um sistema real corporativo, essa senha seria enviada apenas para o e-mail do usuário!
        return await Response(new 
        { 
            Mensagem = "Senha resetada. Utilize a senha provisória no próximo login.", 
            SenhaTemporaria = command.SenhaProvisoriaGerada 
        });
    }
}