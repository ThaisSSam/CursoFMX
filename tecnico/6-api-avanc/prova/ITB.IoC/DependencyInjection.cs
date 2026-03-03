using FluentValidation;
using FluentValidation.AspNetCore;
using ITB.Application.Commands;
using ITB.Application.Handlers;
using ITB.Application.Validations;
using ITB.Domain.Core.Commands;
using ITB.Domain.Core.Messages.Interfaces; // Ajustado para sua interface do Mediator
using ITB.Domain.Interfaces;
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
        // 1. Registra todos os validadores do Assembly de Application automaticamente
        services.AddValidatorsFromAssemblyContaining<AdicionarVeiculoValidation>();

        // 2. Habilita a validação automática para o ASP.NET
        services.AddFluentValidationAutoValidation();

        // 1. Banco de Dados (PostgreSQL)
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));

        // 2. Repositórios
        services.AddScoped<ITB.Domain.Interfaces.IMarcaRepository, ITB.Infrastructure.Repositories.MarcaRepository>();
        services.AddScoped<ITB.Domain.Interfaces.IVeiculoRepository, ITB.Infrastructure.Repositories.VeiculoRepository>();
        services.AddScoped<ITB.Domain.Interfaces.IModeloRepository, ITB.Infrastructure.Repositories.ModeloRepository>();
        // --------------------------------

        // 3. Registramos o Unit of Work com tempo de vida SCOPED (A mesma duração do DbContext e da requisição HTTP)
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // 4. Barramento de Mensagens (Mediator)
        services.AddScoped<IMessageBus, InMemoryBus>();

        // 5. Handlers do CRUD de Marca
        services.AddScoped<IHandler<AdicionarMarcaCommand>, AdicionarMarcaHandler>();
        services.AddScoped<IHandler<AdicionarVeiculoCommand>, AdicionarVeiculoHandler>();
        
        services.AddScoped<IHandler<AtualizarVeiculoCommand>, AtualizarVeiculoHandler>();
        services.AddScoped<IHandler<DesativarVeiculoCommand>, DesativarVeiculoHandler>();
        services.AddScoped<IHandler<AdicionarModeloCommand>, AdicionarModeloHandler>();
        // services.AddScoped<IHandler<AtualizarMarcaCommand>, AtualizarMarcaHandler>();
        // services.AddScoped<IHandler<DeletarMarcaCommand>, DeletarMarcaHandler>();

        // 6. Handlers de Log (Opcional - se você quiser ver o log no console)
        services.AddScoped(typeof(IHandler<>), typeof(LogComandoGenericoHandler<>));

        // 7. Log Genérico (Open Generics) - O "toque de mestre" do seu outro projeto
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