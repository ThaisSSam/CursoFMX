using ITB.Api.Filters;
using ITB.API.Middleware;
using ITB.IoC;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.RateLimiting;
using ITB.API.Extensions;
using Microsoft.AspNetCore.Diagnostics.HealthChecks; // Adicione este using no topo! 

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddHttpContextAccessor();

// builder.Services.AddApiRateLimiting();
builder.Services.AddAntiSpam();

builder.Services.AddOutputCache();

// // Antes do builder.Build()... 
// builder.Services.AddRateLimiter(options => 
// { 
//     // Criamos uma regra chamada "CatracaPadrao" 
//     options.AddFixedWindowLimiter("CatracaPadrao", regras => 
//     { 
//         regras.PermitLimit = 5; // Limite de 5 acessos 
//         regras.Window = TimeSpan.FromSeconds(10); // A cada 10 segundos 
//         regras.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst; 
//         regras.QueueLimit = 0; // Se passar de 5, não cria fila de espera. Rejeita na hora! 
//     }); 

//     // Customizando a mensagem de erro (Opcional, mas recomendado) 
//     options.RejectionStatusCode = StatusCodes.Status429TooManyRequests; 
// });

builder.Services.AddCors(options =>
{
    options.AddPolicy("PoliticaRestrita", policy =>
    {
        policy.WithOrigins(
            "http://localhost:3000",
            "https://localhost:7021",

            "https://example.com"
        )
        .WithMethods("GET", "POST", "PUT")
        .AllowAnyHeader();
    });
});

builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen(c => 
// { 

//     c.SwaggerDoc("v1", new OpenApiInfo() { Title = "API Concessionária", Version = "v1" }); 

//     // Avisa o Swagger que existe um Header exigido 
//     c.AddSecurityDefinition("ApiKey", new Microsoft.OpenApi.Models.OpenApiSecurityScheme 
//     { 
//         Description = "Insira a sua API Key secreta no campo abaixo.", 
//         Name = "X-API-KEY", // Tem que ser exatamente o mesmo nome do filtro! 
//         In = ParameterLocation.Header, 
//         Type = SecuritySchemeType.ApiKey, 
//         Scheme = "ApiKeyScheme" 
//     });

//     c.AddSecurityDefinition("Super-Token", new Microsoft.OpenApi.Models.OpenApiSecurityScheme 
//     { 
//         Description = "Insira a sua API Key secreta no campo abaixo.", 
//         Name = "X-Super-Token", // Tem que ser exatamente o mesmo nome do filtro! 
//         In = ParameterLocation.Header, 
//         Type = SecuritySchemeType.ApiKey, 
//         Scheme = "ApiKeyScheme" 
//     }); 

//     // Aplica a exigência globalmente na interface 
//     c.AddSecurityRequirement(new OpenApiSecurityRequirement 
//     { 
//         { 
//             new OpenApiSecurityScheme 
//             { 
//                 Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, 
//                     Id = "ApiKey" }, 
//                 Scheme = "oauth2", 
//                 Name = "ApiKey", 
//                 In = ParameterLocation.Header, 
//             }, 
//             new List<string>() 
//         } 
//     });

//     c.AddSecurityRequirement(new OpenApiSecurityRequirement 
//     { 
//         { 
//             new OpenApiSecurityScheme 
//             { 
//                 Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, 
//                     Id = "Super-Token" }, 
//                 Scheme = "oauth2", 
//                 Name = "Super-Token", 
//                 In = ParameterLocation.Header, 
//             }, 
//             new List<string>() 
//         } 
//     }); 
// });

// 4. Configuração Avançada do Swagger (Múltiplas Camadas de Segurança)
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API Concessionária", Version = "v1" });

    // 🔐 CADEADO 1: A API Key (Proteção do Aplicativo)
    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = "Insira a sua API Key do App Oficial.",
        Name = "X-API-KEY", // ATENÇÃO: Tem que ser o mesmo nome que o seu filtro espera ler!
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "ApiKeyScheme"
    });

    // 🔐 CADEADO 2: O JWT (Proteção do Usuário)
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Cole APENAS o seu token JWT aqui (o Swagger colocará o 'Bearer ' automaticamente).",

        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    // 🎯 APLICAÇÃO: Diz para o Swagger enviar os DOIS cabeçalhos em todas as requisições
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        // Exigência 1: Mandar o X-API-KEY
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "ApiKey" },
                Scheme = "oauth2",
                Name = "ApiKey",
                In = ParameterLocation.Header
            },
            Array.Empty<string>()
        },
        // Exigência 2: Mandar o Authorization (Bearer)
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            Array.Empty<string>()
        }
    });
});

builder.Host.AddSerilogApi();

//Registrando o Handler 
// Deve vir antes do builder.Build(). Pode ficar junto com os outros Add... 
builder.Services.AddExceptionHandler<GlobalExceptionHandler>(); 
builder.Services.AddProblemDetails(); // IMPORTANTE: No .NET 8, isso é obrigatório para o Handler funcionar! 

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
    options.Filters.Add<ApiKeyAttribute>();
});

var app = builder.Build();

// app.UseMiddleware<ExceptionMiddleware>();
// Deve ser o primeiro middleware de negócio, ANTES da segurança e das controllers! 
app.UseExceptionHandler(); 

app.UseHttpsRedirection();

app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseRateLimiter();

app.MapControllers().RequireAuthorization();

// Define a rota (endereço) onde o teste ficará disponível e abre as configurações customizadas
app.MapHealthChecks("/health", new HealthCheckOptions
{
    // Intercepta a resposta original do .NET para podermos formatar do nosso jeito.
    // Recebe o 'context' (a requisição web) e o 'report' (o laudo médico gerado pelo .NET).
    ResponseWriter = async (context, report) =>
    {
        // Avisa ao navegador que o texto que vamos devolver é um formato JSON.
        context.Response.ContentType = "application/json";
        // Cria um objeto anônimo (sob medida) para desenhar a estrutura exata do nosso JSON.
        var resposta = new
        {
            // Pega o status geral de tudo (ex: se 1 teste falhar, o geral fica "Unhealthy").
            StatusGeral = report.Status.ToString(),
            // Marca o tempo total que a API demorou para fazer todos os testes.
            TempoDeResposta = report.TotalDuration,
            // Entra na lista de testes individuais (Entries) e transforma cada um (Select) num sub-bloco.
            Dependencias = report.Entries.Select(e => new
            {
                // Pega o "apelido" que demos ao teste lá na configuração (ex: "BancoDeDados_Postgres").
                Nome = e.Key,
                // Pega o status específico somente desse item testado.
                Status = e.Value.Status.ToString(),
                // --- INÍCIO DA REGRA DA MENSAGEM ---
                // Faz uma pergunta de verificação (Operador Ternário): O status deste item é 'Healthy' (Saudável)?
                Msg = e.Value.Status == Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Healthy
                        // Se a resposta for SIM, imprime a mensagem de sucesso definida por nós.
                        ? "API e BANCO Online"
                        // Se a resposta for NÃO, ele tenta pegar o erro original (Exception). 
                        // Se a Exception for nula (??), tenta pegar a descrição do Entity Framework.
                        // Se a descrição também for nula (??), usa a mensagem padrão de falha.
                        : (e.Value.Exception?.Message ?? e.Value.Description ?? "Falha ao conectar na dependência.")
            }) // Fim do mapeamento das dependências
        }; // Fim do objeto 'resposta'
        // Escreve fisicamente o nosso objeto 'resposta' na tela do cliente como um JSON válido.
        await context.Response.WriteAsJsonAsync(resposta);
    }
});

app.UseCors("PoliticaRestrita");

app.UseOutputCache();

app.Run();