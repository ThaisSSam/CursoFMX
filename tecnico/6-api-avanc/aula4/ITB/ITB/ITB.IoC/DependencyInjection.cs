using ITB.Application.Interfaces;
using ITB.Application.Mappings;
using ITB.Application.Services;
using ITB.Domain.Entities;
using ITB.Domain.Interfaces;
using ITB.Infrastructure.Persistence;
using ITB.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

        // 3. Serviços da Aplicação
        services.AddScoped<IFabricanteService, FabricanteService>();
        // services.AddScoped<IUserService, UserService>();

        services.AddScoped<ICategoriaRepository, CategoriaRepository>();

        services.AddScoped<ICategoriaService, CategoriaService>();

        // 5. AutoMapper
        services.AddAutoMapper(cfg => 
        {
            cfg.AddProfile<MappingProfile>();
        }, typeof(MappingProfile).Assembly);

        return services;
    }
}