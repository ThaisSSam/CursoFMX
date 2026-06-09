namespace Treinamento.API.Controllers; 

public sealed class LoginRequest
{    
    public required string Email { get; init; }
    public required string Senha { get; init; } 
    public bool LembrarAcesso { get; init; }
}