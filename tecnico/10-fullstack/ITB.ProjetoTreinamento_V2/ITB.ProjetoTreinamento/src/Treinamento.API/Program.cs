using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Serilog;
using Treinamento.API.Configurations;
using Treinamento.API.Validacao;
using Treinamento.IoC;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

builder.AddConfiguration();
builder.Services.ResolveDependenciesApp();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.WebApiConfig();
builder.Services.AddSwaggerConfig();

builder.Services
    .AddFluentValidationAutoValidation()
    .AddValidatorsFromAssemblyContaining<PlaceholderValidator>();

var app = builder.Build();

Console.WriteLine($"\n>>>> HASH SEGURO DO BCRYPT: {BCrypt.Net.BCrypt.HashPassword("12345")}\n");

var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerConfig(provider);
}

app.UseWebApiConfig(app.Environment);

app.MapControllers();

app.Run();

public partial class Program;
