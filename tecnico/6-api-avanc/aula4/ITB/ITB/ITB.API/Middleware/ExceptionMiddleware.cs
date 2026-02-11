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
            _logger.LogError(ex, "Erro não tratado detectado");
            await HandleExceptionAsync(context, ex);
        }
    }
    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = 500;

        var response = new
        {
            Error = "Algo deu errado internamente",
            // Message = exception.Message,
            Message = "Mensagem alterada para não ficar grande", 
            Timestamp = DateTime.Now,
            innerException= "menor",
        };
        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
