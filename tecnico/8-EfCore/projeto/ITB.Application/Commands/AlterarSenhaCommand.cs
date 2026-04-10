using System;
using ITB.Domain.Core.Messages.Interfaces;

namespace ITB.Application.Commands;

public class AlterarSenhaCommand : ICommand 
{ 
    // O identificador do usuário (A Controller extrai isso do Token JWT e preenche aqui) 
    public int UsuarioId { get; set; }  
    
    // Os dados que o usuário digitou na tela do Front-End 
    public string SenhaAtual { get; set; } = string.Empty; 
    public string NovaSenha { get; set; } = string.Empty; 
} 