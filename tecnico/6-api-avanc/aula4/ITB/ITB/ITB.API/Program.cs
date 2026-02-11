using ITB.API.Middleware;
using ITB.IoC; // Essencial para o método de extensão
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// 1. Configuração do Serilog
// Log.Logger = new LoggerConfiguration()
//     .ReadFrom.Configuration(builder.Configuration)
//     .Enrich.FromLogContext()
//     .WriteTo.Console()
//     .CreateLogger();

// builder.Host.UseSerilog();

builder.Host.AddSerilogApi();

// 2. Injeção de Dependência (Chama o seu projeto ITB.IoC)
// Isso já registra Banco, Repositórios, Serviços e AutoMapper
builder.Services.AddInfrastructure(builder.Configuration);

// 3. Controllers
builder.Services.AddControllers();

// 4. Swagger Simples (Sem as configurações de cadeado/Bearer)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 5. CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

// PIPELINE

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.MapControllers();

app.UseAuthorization();

app.Run();