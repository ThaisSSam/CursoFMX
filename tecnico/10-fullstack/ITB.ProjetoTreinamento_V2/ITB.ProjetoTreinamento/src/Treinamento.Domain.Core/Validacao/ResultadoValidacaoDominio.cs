namespace Treinamento.Domain.Core.Validacao;

public class ResultadoValidacaoDominio
{
    public List<ErroValidacaoDominio> Erros { get; } = [];

    public bool Valido => Erros.Count == 0;
}
