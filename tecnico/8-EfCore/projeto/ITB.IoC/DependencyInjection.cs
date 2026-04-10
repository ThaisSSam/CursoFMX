using FluentValidation;
using FluentValidation.AspNetCore;
using ITB.Application.Commands;
using ITB.Application.Handlers;
using ITB.Application.Interfaces;
using ITB.Application.Validations;
using ITB.Domain.Core.Messages;
using ITB.Domain.Core.Messages.Interfaces;
using ITB.Domain.Core.Notifications;
using ITB.Domain.Entities;
using ITB.Domain.Interfaces;
using ITB.Infrastructure.Bus;
using ITB.Infrastructure.Persistence;
using ITB.Infrastructure.Queries;
using ITB.Infrastructure.Repositories;
using ITB.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace ITB.IoC;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddValidatorsFromAssemblyContaining<AdicionarVeiculoValidation>();
        services.AddValidatorsFromAssemblyContaining<AdicionarMarcaValidation>();
        services.AddValidatorsFromAssemblyContaining<AdicionarModeloValidation>();

        services.AddFluentValidationAutoValidation();

        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<AppDbContext>(Options => Options.UseNpgsql(connectionString)
            .UseSnakeCaseNamingConvention());

        services.AddHealthChecks()
            .AddDbContextCheck<AppDbContext>("BancoDedados_Postgres");

        #region Repositorios
        services.AddScoped<IMarcaRepository, MarcaRepository>();
        services.AddScoped<IVeiculoRepository, VeiculoRepository>();
        services.AddScoped<IModeloRepository, ModeloRepository>();
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        services.AddScoped<IUsuarioReadRepository, UsuarioReadRepository>();
        services.AddScoped<IUsuarioWriteRepository, UsuarioWriteRepository>();
        #endregion

        #region Handlers
        services.AddScoped<IHandler<AdicionarMarcaCommand>, AdicionarMarcaHandler>();
        services.AddScoped<IHandler<DeletarMarcaCommand>, DeletarMarcaHandler>();

        services.AddScoped<IHandler<AdicionarVeiculoCommand>, AdicionarVeiculoHandler>();
        services.AddScoped<IHandler<AtualizarVeiculoCommand>, AtualizarVeiculoHandler>();
        services.AddScoped<IHandler<DesativarVeiculoCommand>, DesativarVeiculoHandler>();
        services.AddScoped<IHandler<AdicionarUsuarioCommand>, AdicionarUsuarioHandler>();

        services.AddScoped<IHandler<AdicionarModeloCommand>, AdicionarModeloHandler>();
        services.AddScoped<IHandler<AtualizarModeloCommand>, AtualizarModeloHandler>();
        services.AddScoped<IHandler<DesativarModeloCommand>, DesativarModeloHandler>();
        services.AddScoped<IHandler<DescontoMarcaVeiculoCommand>, DescontoMarcaVeiculoHandler>();
        services.AddScoped<RealizarLoginHandler>();
        #endregion;

        #region Queries
        services.AddScoped<IVeiculoQuery, VeiculoQuery>();
        services.AddScoped<IMarcaQuery, MarcaQuery>();
        services.AddScoped<IModeloQuery, ModeloQuery>();
        #endregion

        #region  Services
        services.AddScoped<ITokenService, TokenService>();
        #endregion;

        services.AddScoped<ExportarVeiculosExcelQueryHandler>();

        services.AddScoped<IDomainNotificationHandler<DomainNotification>, DomainNotificationHandler>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IMessageBus, InMemoryBus>();
        services.AddScoped(typeof(IHandler<>), typeof(LogComandoGenericoHandler<>));

        return services;
    }

    public static void AddSerilogApi(this IHostBuilder host)
    {
        host.UseSerilog((context, config) =>
        {
            config.ReadFrom.Configuration(context.Configuration)
            .WriteTo.Console()
            .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day);
        });
    }
}
