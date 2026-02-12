using System;
using System.Text.Json;

namespace ITB.API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, "Erro não tratado detectado");
            await HandleExceptionAsync(context, ex);
        }
    }
    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = 500;

        string mensagemCurta = "Algo deu errado internamente";

        if (exception.InnerException is Npgsql.PostgresException postgresEx && postgresEx.SqlState == "22001")
        {
            mensagemCurta = "Erro: O valor enviado é muito longo para o campo (máximo 14 caracteres).";
        }
        else if (exception.Message.Contains("22001"))
        {
            mensagemCurta = "Erro: valor é muito longo para tipo character varying(14)";
        }
        var response = new
        {
            Error = "FALHA CRÍTICA",
            Message = mensagemCurta, 
            Timestamp = DateTime.Now
        };
        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
