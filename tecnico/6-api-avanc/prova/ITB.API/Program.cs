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
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers(options =>
{
  // Adiciona o filtro globalmente
  options.Filters.Add<ValidationFilter>();
  // Adiciona a API Key globalmente
  options.Filters.Add<ApiKeyAttribute>();
});

builder.Services.AddDbContext<AppDbContext>(options => options
    .UseNpgsql(connectionString)
    .UseSnakeCaseNamingConvention());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
  c.SwaggerDoc("v1", new OpenApiInfo { Title = "API Concessionária", Version = "v1" });

  // Avisa o Swagger que existe um Header exigido 
  c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
  {
    Description = "Insira a sua API Key secreta no campo abaixo.",
    Name = "X-API-KEY", // Tem que ser exatamente o mesmo nome do filtro! 
    In = ParameterLocation.Header,
    Type = SecuritySchemeType.ApiKey,
    Scheme = "ApiKeyScheme"
  });

  // Aplica a exigência globalmente na interface 
  c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
      {
        new OpenApiSecurityScheme
        {
          Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "ApiKey" },
          Scheme = "oauth2",
          Name = "ApiKey",
          In = ParameterLocation.Header,
        },
        new List<string>()
      }
    });
});

builder.Host.AddSerilogApi();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddCors(options =>
{
  options.AddPolicy("PoliticaRestrita", policy =>
  {
    policy.WithOrigins(
          "http://localhost:3000",
          "https://meu-front-oficial.com",
          "https://example.com"
      )
      .WithMethods("GET", "POST", "PUT")
      .AllowAnyHeader();
  });
});

// builder.Services.AddRateLimiter(options =>
// {
//   // Criamos uma regra chamada "CatracaPadrao" 
//   options.AddFixedWindowLimiter("CatracaPadrao", regras =>
//   {
//     regras.PermitLimit = 5; // Limite de 5 acessos 
//     regras.Window = TimeSpan.FromSeconds(10); // A cada 10 segundos 
//     regras.QueueProcessingOrder =
// System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
//     regras.QueueLimit = 0; // Se passar de 5, não cria fila de espera. Rejeita na hora! 
//   });

//   // Customizando a mensagem de erro (Opcional, mas recomendado) 
//   options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
// });

builder.Services.AddApiPoliticalLimiting();

builder.Services.AddOutputCache();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>(); // Captura erros globais 

app.UseHttpsRedirection();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.UseHttpsRedirection();

app.UseCors("PoliticaRestrita");

app.UseOutputCache();

app.UseRouting();

app.UseRateLimiter();

app.UseAuthorization();

app.MapControllers();

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

