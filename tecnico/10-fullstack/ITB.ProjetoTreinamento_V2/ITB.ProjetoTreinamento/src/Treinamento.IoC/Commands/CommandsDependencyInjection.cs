using Microsoft.Extensions.DependencyInjection;
using Treinamento.Domain.Aggregates.Sistema.Commands;
using Treinamento.Domain.Core.Events;

namespace Treinamento.IoC.Commands;

internal static class CommandsDependencyInjection
{
    internal static IServiceCollection AddCommands(this IServiceCollection services)
    {
        services.AddScoped<IHandler<PingCommand>, PingCommandHandler>();
        return services;
    }
}
