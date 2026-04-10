using System;

namespace ITB.Application.Dtos;

public class AdicionarUsuarioRequest 
{ 
    public string Nome { get; set; } = string.Empty; 
    public string Email { get; set; } = string.Empty; 
    public string SenhaHash { get; set; } = string.Empty; 
    public string Perfil { get; set; } = string.Empty; 
} 
