using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Treinamento.CrossCutting;
using Treinamento.Domain.Core.Interfaces;
using Treinamento.Infrastructure.Persistence;
using Treinamento.Infrastructure.UoW;

namespace Treinamento.IoC.Database;

internal static class DatabaseDependencyInjection
{
    private const int MaxRetryCount = 5;
    private const int MaxRetryDelaySeconds = 5;
    private const int CommandTimeoutSeconds = 30;

    internal static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services.AddWriteDatabase();
        services.AddReadDatabase();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }

    private static IServiceCollection AddWriteDatabase(this IServiceCollection services)
    {
        services.AddDbContext<TreinamentoWriteContext>((_, options) =>
        {
            ConfigurePostgreSqlContext(options, Configuration.WriteConnectionString, QueryTrackingBehavior.TrackAll);
        });

        return services;
    }

    private static IServiceCollection AddReadDatabase(this IServiceCollection services)
    {
        services.AddDbContext<TreinamentoReadContext>((_, options) =>
        {
            ConfigurePostgreSqlContext(options, Configuration.ReadConnectionString, QueryTrackingBehavior.NoTracking);
        });

        return services;
    }

    private static void ConfigurePostgreSqlContext(
        DbContextOptionsBuilder options,
        string connectionString,
        QueryTrackingBehavior trackingBehavior)
    {
        options
            .UseNpgsql(connectionString, npgsql =>
            {
                npgsql.EnableRetryOnFailure(
                    maxRetryCount: MaxRetryCount,
                    maxRetryDelay: TimeSpan.FromSeconds(MaxRetryDelaySeconds),
                    errorCodesToAdd: null);
                npgsql.CommandTimeout(CommandTimeoutSeconds);
                npgsql.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            })
            .UseQueryTrackingBehavior(trackingBehavior);
    }
}
