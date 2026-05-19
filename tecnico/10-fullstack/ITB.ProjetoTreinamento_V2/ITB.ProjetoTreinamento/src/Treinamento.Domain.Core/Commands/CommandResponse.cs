namespace Treinamento.Domain.Core.Commands;

public class CommandResponse(bool success, string? errorMessage = null)
{
    public bool Success { get; } = success;
    public string? ErrorMessage { get; } = errorMessage;
}
