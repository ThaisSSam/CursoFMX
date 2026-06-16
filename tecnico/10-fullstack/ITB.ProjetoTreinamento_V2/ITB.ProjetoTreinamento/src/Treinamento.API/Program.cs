using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Serilog;
using Treinamento.API.Configurations;
using Treinamento.API.Validacao;
using Treinamento.IoC;

var builder = WebApplication.CreateBuilder(args);

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

builder.AddConfiguration();
builder.Services.ResolveDependenciesApp();

builder.Services.AddCors(options =>
{
    options.AddPolicy("FmxReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:5174")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.WebApiConfig();
builder.Services.AddSwaggerConfig();

builder.Services
    .AddFluentValidationAutoValidation()
    .AddValidatorsFromAssemblyContaining<PlaceholderValidator>();

var app = builder.Build();

var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerConfig(provider);
}

app.UseCors("FmxReactApp");

app.UseWebApiConfig(app.Environment);

app.MapControllers();

app.Run();

public partial class Program;
