using ITB.Application.Commands;
using ITB.Application.Handlers;
using ITB.Application.Interfaces;
using ITB.Application.Mappings;
using ITB.Application.Services;
using ITB.Domain.Core.Messages;
using ITB.Domain.Entities;
using ITB.Domain.Interfaces;
using ITB.Infrastructure.Bus;
using ITB.Infrastructure.Persistence;
using ITB.Infrastructure.Persistence.Repositories;
using ITB.Infrastructure.Repositories;
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
        // 1. Banco de Dados (PostgreSQL)
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));

        // 2. Repositórios
        services.AddScoped<IFabricanteRepository, FabricanteRepository>();
        // services.AddScoped<IUserRepository, UserRepository>();

        #region Serviços da Aplicação
        services.AddScoped<IFabricanteService, FabricanteService>();
        // services.AddScoped<IUserService, UserService>();

        services.AddScoped<ICategoriaRepository, CategoriaRepository>();

        services.AddScoped<ICategoriaService, CategoriaService>();

        services.AddScoped<IProdutoRepository, ProdutoRepository>();

        services.AddScoped<IProdutoService, ProdutoService>();

        services.AddScoped<IMessageBus, InMemoryBus>();
        #endregion

        #region Handlers
        services.AddScoped<IHandler<CriarFabricanteCommand>, CriarFabricanteHandler>();
        services.AddScoped<IHandler<CriarFabricanteCommand>, LogFabricanteHandler>();
        #endregion

        #region Log Genérico
        services.AddScoped(typeof(IHandler<>), typeof(LogComandoGenericoHandler<>));
        #endregion

        #region AutoMapper
        services.AddAutoMapper(cfg => 
        {
            cfg.AddProfile<MappingProfile>();
        }, typeof(MappingProfile).Assembly);
        #endregion

        

        return services;
    }

    public static void AddSerilogApi(this IHostBuilder host)
    {
        host.UseSerilog((context, configuration)=>
        {
            configuration
                .WriteTo.Console()
                .WriteTo.File("logs/log-.txt",rollingInterval: RollingInterval.Day)
                .ReadFrom.Configuration(context.Configuration);
        });
    }
}