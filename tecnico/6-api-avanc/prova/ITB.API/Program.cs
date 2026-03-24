using ITB.IoC;
using ITB.API.Middleware;
using ITB.API.Filters;
using ITB.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

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

app.UseCors("PoliticaRestrita");

app.MapControllers();

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

