using FluentValidation;
using FluentValidation.AspNetCore;
using ITB.Application.Commands;
using ITB.Application.Handlers;
using ITB.Application.Interfaces;
using ITB.Application.Validations;
using ITB.Domain.Core.Commands;
using ITB.Domain.Core.Messages.Interfaces;
using ITB.Domain.Interfaces;
using ITB.Infrastructure.Bus;
using ITB.Infrastructure.Persistence;
using ITB.Infrastructure.Queries;
using ITB.Infrastructure.Repositories;
using ITB.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace ITB.IoC;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // 1. Fluent Validation
        services.AddValidatorsFromAssemblyContaining<AdicionarVeiculoValidation>();
        services.AddFluentValidationAutoValidation();

        // 2. Banco de Dados (PostgreSQL)
        // Ajustado para 'DefaultConnection' conforme seu appsettings.json
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddHealthChecks().AddDbContextCheck<AppDbContext>("BancoDeDados_Postgres");

        // 3. Repositórios
        services.AddScoped<IMarcaRepository, MarcaRepository>();
        services.AddScoped<IVeiculoRepository, VeiculoRepository>();
        services.AddScoped<IModeloRepository, ModeloRepository>();
        services.AddScoped<IUsuarioRepository, UsuarioRepository>(); // Repositório para o Login
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // 4. Barramento de Mensagens (Mediator Pattern)
        services.AddScoped<IMessageBus, InMemoryBus>();

        // 5. Queries (Leitura)
        services.AddScoped<IVeiculoQuery, VeiculoQuery>();
        services.AddScoped<IModeloQuery, ModeloQuery>();
        services.AddScoped<IMarcaQuery, MarcaQuery>();

        // 6. Serviços de Infraestrutura
        services.AddScoped<ITokenService, TokenService>();

        // 7. Handlers (Comandos/Lógica de Negócio)
        // Registro dos Handlers seguindo a interface IHandler
        services.AddScoped<IHandler<RealizarLoginCommand>, RealizarLoginHandler>();
        services.AddScoped<IHandler<AdicionarMarcaCommand>, AdicionarMarcaHandler>();
        services.AddScoped<IHandler<AdicionarVeiculoCommand>, AdicionarVeiculoHandler>();
        services.AddScoped<IHandler<AtualizarVeiculoCommand>, AtualizarVeiculoHandler>();
        services.AddScoped<IHandler<AdicionarModeloCommand>, AdicionarModeloHandler>();
        services.AddScoped<ExportarVeiculosExcelQueryHandler>();

        // 8. Registro específico para a Controller (Caso ela peça a classe concreta)
        services.AddScoped<RealizarLoginHandler>();

        // 9. Handlers Genéricos (Logs e Decorators)
        services.AddScoped(typeof(IHandler<>), typeof(LogComandoGenericoHandler<>));

        services.AddHttpContextAccessor();

        return services;
    }

    public static void AddSerilogApi(this IHostBuilder host)
    {
        host.UseSerilog((context, configuration) =>
        {
            configuration
                .WriteTo.Console()
                .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
                .ReadFrom.Configuration(context.Configuration);
        });
    }
}