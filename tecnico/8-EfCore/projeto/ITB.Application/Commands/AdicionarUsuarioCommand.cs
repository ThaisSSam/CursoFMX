using System;
using ITB.Domain.Core.Messages.Interfaces;

namespace ITB.Application.Commands;

public class AdicionarUsuarioCommand : ICommand 
{ 
    public string Nome { get; set; } = string.Empty; 
    public string Email { get; set; } = string.Empty; 
    public string Senha { get; set; } = string.Empty; 
    public string Perfil { get; set; } = string.Empty; 
    
    // Propriedade para devolver o ID gerado para a Controller 
    public int? IdGerado { get; set; }  
}