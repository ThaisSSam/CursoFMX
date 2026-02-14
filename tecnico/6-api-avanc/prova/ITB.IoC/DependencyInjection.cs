using ITB.Application.Commands;
using ITB.Application.Handlers;
using ITB.Domain.Core.Messages.Interfaces; // Ajustado para sua interface do Mediator
using ITB.Infrastructure.Bus;
using ITB.Infrastructure.Persistence;
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
        services.AddScoped<ITB.Domain.Interfaces.IFabricanteRepository, ITB.Infrastructure.Repositories.FabricanteRepository>();
        // --------------------------------

        // 3. Barramento de Mensagens (Mediator)
        services.AddScoped<IMessageBus, InMemoryBus>();

        // 4. Handlers do CRUD de Fabricante
        services.AddScoped<IHandler<CriarFabricanteCommand>, CriarFabricanteHandler>();
        services.AddScoped<IHandler<AtualizarFabricanteCommand>, AtualizarFabricanteHandler>();
        services.AddScoped<IHandler<DeletarFabricanteCommand>, DeletarFabricanteHandler>();

        // 5. Handlers de Log (Opcional - se você quiser ver o log no console)
        services.AddScoped(typeof(IHandler<>), typeof(LogComandoGenericoHandler<>));

        // 6. Log Genérico (Open Generics) - O "toque de mestre" do seu outro projeto
        services.AddScoped(typeof(IHandler<>), typeof(LogComandoGenericoHandler<>));

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