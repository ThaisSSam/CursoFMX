namespace Treinamento.Domain.Core.Validacao;

public class ErroValidacaoDominio(string mensagemErro, string? nomePropriedade = null)
{
    public string MensagemErro { get; } = mensagemErro;
    public string? NomePropriedade { get; } = nomePropriedade;
}
