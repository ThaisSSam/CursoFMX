/// <summary>
/// A caixa oficial do nosso sistema. Tudo o que sai da nossa API para a internet estará aqui dentro.
/// O <T> permite que a propriedade Data receba qualquer coisa (um objeto, uma lista, etc).
/// </summary>
public class ApiResponse<T>
{
    // Sinalizador imediato para o Front-End saber se pinta a tela de verde (Sucesso) ou vermelho (Erro)
    public bool Success { get; set; }

    // A carga útil. Se inserimos um veículo, devolvemos o ID ou o objeto inteiro aqui.
    public T? Data { get; set; }

    // Mensagem principal para o usuário (ex: "Veículo cadastrado!")
    public string? Message { get; set; }

    // Lista contendo os detalhes dos erros (extraída do nosso bloco de notas)
    public List<string>? Errors { get; set; } = [];

    // Construtor 1: O CAMINHO FELIZ (Usado quando a requisição deu certo)
    public ApiResponse(T dados, string? mensagem = null)
    {
        Success = true;
        Data = dados;
        Message = mensagem;
    }

    // Construtor 2: O CAMINHO TRISTE (Usado quando a requisição esbarra em regras de negócio)
    public ApiResponse(string mensagem, List<string>? erros = null)
    {
        Success = false;
        Message = mensagem;
        Errors = erros ?? []; // O '?? []' garante que a lista nunca seja nula, evitando bugs no Front-End
    }
}