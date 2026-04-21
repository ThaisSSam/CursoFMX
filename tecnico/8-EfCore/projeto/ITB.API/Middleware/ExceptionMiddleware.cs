using System.Net;
using System.Text.Json;
using ITB.Domain.Core.Exceptions;

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
        catch (DomainException ex)
        {
            _logger.LogWarning($"ERRO DE DOMÍNIO -> Local: {context.GetEndpoint()} | Erro: {ex.Message}");
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 400;

            var response = new
            {
                Error = "Ops! Algo deu errado.",
                Message = ex.Message,
                Timestamp = DateTime.Now
            };

            await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError);
        }
        catch (Exception ex)
        {
            _logger.LogError($"FALHA CRÍTICA -> Local: {context.GetEndpoint()} | Erro: {ex.InnerException?.Message}");
            await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception, HttpStatusCode statusCode)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            Error = "Ops! Algo deu errado.",
            Message = exception.InnerException?.Message,
            Timestamp = DateTime.Now
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
