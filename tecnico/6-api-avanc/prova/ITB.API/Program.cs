using ITB.IoC;
using ITB.API.Middleware;
using ITB.API.Filters;
using ITB.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.RateLimiting;
using ITB.API.Extensions;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// 1. Configurações de Serviços
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
    options.Filters.Add<ApiKeyAttribute>();
});

// REMOVIDO: builder.Services.AddDbContext daqui pois já está dentro de AddInfrastructure
builder.Services.AddJwtAuthentication(builder.Configuration); 
builder.Services.AddHttpContextAccessor(); 
builder.Services.AddEndpointsApiExplorer();

// 2. Configuração do Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API Concessionária", Version = "v1" });

    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = "Insira a sua API Key do App Oficial.",
        Name = "X-API-KEY",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "ApiKeyScheme"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Cole APENAS o seu token JWT aqui.",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "ApiKey" },
                Scheme = "oauth2", Name = "ApiKey", In = ParameterLocation.Header
            },
            Array.Empty<string>()
        },
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
                Scheme = "oauth2", Name = "Bearer", In = ParameterLocation.Header
            },
            Array.Empty<string>()
        }
    });
});

builder.Host.AddSerilogApi();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("PoliticaRestrita", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .WithMethods("GET", "POST", "PUT")
              .AllowAnyHeader();
    });
});

builder.Services.AddApiPoliticalLimiting();
builder.Services.AddOutputCache();

var app = builder.Build();

// 3. Definição da variável que estava faltando (Resolve o erro CS0103)
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

// 4. Pipeline de Execução (Middlewares)
app.UseMiddleware<ExceptionMiddleware>(); 

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("PoliticaRestrita");
app.UseRouting();

app.UseAuthentication();  
app.UseAuthorization();

app.UseOutputCache();
app.UseRateLimiter();

app.MapControllers();

// 5. HealthChecks
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var resposta = new
        {
            StatusGeral = report.Status.ToString(),
            TempoDeResposta = report.TotalDuration,
            Dependencias = report.Entries.Select(e => new
            {
                Nome = e.Key,
                Status = e.Value.Status.ToString(),
                Msg = e.Value.Status == Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Healthy
                    ? "API e BANCO Online"
                    : (e.Value.Exception?.Message ?? e.Value.Description ?? "Falha ao conectar.")
            })
        };
        await context.Response.WriteAsJsonAsync(resposta);
    }
});

// 6. Rota WeatherForecast (Agora com 'summaries' existindo no contexto)
app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}