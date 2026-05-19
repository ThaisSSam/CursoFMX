namespace Treinamento.API.Controllers.Base;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string? Message { get; set; }
    public List<string>? Errors { get; set; } = [];

    public ApiResponse(T dados, string? mensagem = null)
    {
        Success = true;
        Data = dados;
        Message = mensagem;
    }

    public ApiResponse(string mensagem, List<string>? erros = null)
    {
        Success = false;
        Message = mensagem;
        Errors = erros ?? [];
    }
}
