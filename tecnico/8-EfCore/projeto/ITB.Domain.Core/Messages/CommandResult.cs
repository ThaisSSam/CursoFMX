namespace ITB.Domain.Core.Messages;

public class CommandResult
{
    public bool Sucesso { get; private set; }
    public string Mensagem { get; private set; }
    public object? Dados { get; private set; }

    public CommandResult(bool sucesso, string mensagem, object? dados = null)
    {
        Sucesso = sucesso;
        Mensagem = mensagem;
        Dados = dados;
    }
}