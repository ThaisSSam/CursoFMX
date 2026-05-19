using Microsoft.Extensions.DependencyInjection;
using Treinamento.IoC.Database;

namespace Treinamento.IoC;

internal static class InfrastructureDependencyInjection
{
    internal static IServiceCollection ResolveDependenciesInfraData(this IServiceCollection services)
    {
        services.AddDatabase();
        return services;
    }
}
