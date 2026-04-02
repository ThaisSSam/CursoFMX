using ITB.Domain.Core.Messages.Interfaces;

namespace ITB.Application.Commands;

public class RealizarLoginCommand : ICommand
{
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;

}