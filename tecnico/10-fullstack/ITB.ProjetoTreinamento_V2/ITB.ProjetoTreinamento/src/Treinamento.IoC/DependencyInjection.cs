using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Treinamento.CrossCutting.Cache;
using Treinamento.Domain.Core.Bus;
using Treinamento.Domain.Core.Cache;
using Treinamento.Domain.Core.Notifications;
using Treinamento.IoC.Bus;
using Treinamento.IoC.Commands;
using Treinamento.IoC.Queries;
using Treinamento.ServiceBus;
using Treinamento.Domain.Aggregates.Usuarios;
using Treinamento.Infrastructure.Repositories;
using Treinamento.Domain.Handlers;
using Treinamento.Domain.Core.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Treinamento.Domain.Aggregates.Tarefa.Interfaces;
using Treinamento.Infrastructure.Persistence.Repositories;

namespace Treinamento.IoC;

public static class DependencyInjection
{
    public static IServiceCollection ResolveDependenciesApp(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();

        services.AddMemoryCache();
        services.AddSingleton<ICacheService, MemoryCacheService>();

        services.AddScoped<IDomainNotificationHandler<DomainNotification>, DomainNotificationHandler>();

        services.AddSingleton<IBusRequestServicesResolver, BusRequestServicesResolver>();
        services.AddScoped<IBus, InMemoryBus>();

        services.ResolveDependenciesInfraData();
        services.AddCommands();
        services.AddQueries();

        services.AddScoped<Infrastructure.Persistence.TreinamentoContext>();

        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        services.AddScoped<ILogarUsuarioHandler, LogarUsuarioHandler>();
        services.AddScoped<ITarefaRepository, TarefaRepository>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,  
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("ChaveSuperSeguraParaOMod10DoTreinamento")),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });

        return services;
    }
}
