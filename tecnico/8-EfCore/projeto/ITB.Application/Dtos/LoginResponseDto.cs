using System;

namespace ITB.Application.Dtos;

public class LoginResponseDto 
{ 
    public string? Token { get; set; } 
    public bool ExigeTrocaDeSenha { get; set; } 
} 