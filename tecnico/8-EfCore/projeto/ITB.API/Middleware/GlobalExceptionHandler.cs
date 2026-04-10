using System;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;

namespace ITB.API.Middleware;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) :
IExceptionHandler
{
  public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception
exception, CancellationToken cancellationToken)
  {
    // 1. Registra o erro real (com Stack Trace) para os desenvolvedores 
    logger.LogError(exception, "Uma exceção não tratada ocorreu: {Mensagem}",
exception.Message);

    // 2. Configura a resposta HTTP para 500 (Erro Interno) 
    httpContext.Response.ContentType = "application/json";
    httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

    // 3. Monta o ApiResponse mantendo o contrato visual com o Front-End 
    var erroParaUsuario = exception is ApplicationException
        ? exception.Message
        : "Ocorreu um erro interno no servidor. Nossa equipe já foi notificada.";

    var resposta = new ApiResponse<object>(
        mensagem: "Houve um problema ao processar sua requisição.",
        erros: [erroParaUsuario]
    );

    var jsonResponse = JsonSerializer.Serialize(resposta, new JsonSerializerOptions
    { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
    await httpContext.Response.WriteAsync(jsonResponse, cancellationToken);

    return true;
  }
}