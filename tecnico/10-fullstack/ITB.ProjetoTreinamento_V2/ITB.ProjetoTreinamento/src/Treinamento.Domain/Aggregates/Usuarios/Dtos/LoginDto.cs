namespace Treinamento.Domain.Aggregates.Usuarios;

public class LoginDto
{
    public required string Email { get; init; }
    public required string Senha { get; init; }
    public bool LembrarAcesso { get; init; }
}