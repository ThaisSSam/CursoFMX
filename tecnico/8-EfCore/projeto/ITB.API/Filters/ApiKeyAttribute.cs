using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)] 
public class ApiKeyAttribute : Attribute, IAsyncActionFilter 
{ 
    private const string ApiKeyHeaderName = "X-API-KEY";
    // public async Task OnActionExecutionAsync(ActionExecutingContext context,
    //     ActionExecutionDelegate next)
    // {
    //     var permiteAnonimos = context.ActionDescriptor.EndpointMetadata
    //         .Any(em => em.GetType() == typeof(AllowAnonymousAttribute));

    //     if (permiteAnonimos)
    //     {
    //         await next();
    //         return;
    //     }

    //     // 1. Verifica se a chave veio no cabeçalho 
    //     if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var
    //         extractedApiKey))
    //     {
    //         context.Result = new UnauthorizedObjectResult("API Key não informada no cabeçalho.");
    //         return;
    //     }

    //     // 2. Pega a Chave Verdadeira salva no User Secrets 
    //     var configuration =
    //         context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
    //     var apiKeyVerdadeira = configuration.GetValue<string>("Seguranca:MinhaApiKey");

    //     // 3. Compara as chaves 
    //     if (!apiKeyVerdadeira.Equals(extractedApiKey))
    //     {
    //         context.Result = new UnauthorizedObjectResult("API Key inválida. Acesso negado.");
    //         return;
    //     }

    //     // Tudo certo! Deixa a requisição seguir para o Controller. 
    //     await next();
    // } 
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // 2. Tenta ler o Header da requisição
        if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var extractedApiKey))
        {
            context.Result = new UnauthorizedObjectResult(new { mensagem = "API Key não fornecida!" });
            return;
        }

        // 3. Como não usamos Injeção no construtor, pegamos o Configuration direto do HttpContext
        var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
        var apiKey = configuration.GetValue<string>("Seguranca:MinhaApiKey");

        // 4. Valida se a chave bate com o User Secrets/appsettings
        if (!apiKey!.Equals(extractedApiKey))
        {
            context.Result = new UnauthorizedObjectResult(new { mensagem = "API Key inválida!" });
            return;
        }

        // 5. Se passou por tudo, deixa a requisição seguir para a Controller
        await next();
    }    
} 