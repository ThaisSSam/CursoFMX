using Microsoft.Extensions.DependencyInjection;
using Treinamento.CrossCutting.Cache;
using Treinamento.Domain.Core.Bus;
using Treinamento.Domain.Core.Cache;
using Treinamento.Domain.Core.Notifications;
using Treinamento.IoC.Bus;
using Treinamento.IoC.Commands;
using Treinamento.IoC.Queries;
using Treinamento.ServiceBus;

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

        return services;
    }
}
