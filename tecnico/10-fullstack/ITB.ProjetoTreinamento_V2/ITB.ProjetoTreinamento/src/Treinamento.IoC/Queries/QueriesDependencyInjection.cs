using Microsoft.Extensions.DependencyInjection;

namespace Treinamento.IoC.Queries;

internal static class QueriesDependencyInjection
{
    internal static IServiceCollection AddQueries(this IServiceCollection services)
    {
        // Módulo 1+: registrar IQueryHandler<,> aqui
        return services;
    }
}
