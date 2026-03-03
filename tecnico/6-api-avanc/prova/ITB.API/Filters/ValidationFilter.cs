using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ITB.API.Filters;

public class ValidationFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ModelState.IsValid)
        {
            var mensagensDeErro = context.ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            context.Result = new BadRequestObjectResult(new { 
                mensagem = "Dados inválidos", 
                erros = mensagensDeErro 
            });
            return;
        }

        await next();
    }
}