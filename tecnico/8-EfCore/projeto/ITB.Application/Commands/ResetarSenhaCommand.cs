using System;
using ITB.Domain.Core.Messages.Interfaces;

namespace ITB.Application.Commands;

public class ResetarSenhaCommand : ICommand 
{ 
    // O dado que o usuário digita na tela de "Esqueci minha senha" 
    public string Email { get; set; } = string.Empty; 
    
    // Essa propriedade NÃO vem do Front-End (JSON).  
    // O nosso Handler vai preenchê-la com a senha provisória gerada pelo Domínio  
    // para que a Controller possa enviar por e-mail (ou mostrar na tela na nossa aula)! 
    public string? SenhaProvisoriaGerada { get; set; }  
} 
