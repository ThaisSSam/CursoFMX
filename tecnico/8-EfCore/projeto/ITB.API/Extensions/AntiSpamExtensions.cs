using System.Threading.RateLimiting;

namespace ITB.API.Extensions;

public static class AntiSpamExtensions
{
    public static IServiceCollection AddAntiSpam(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.AddPolicy("PadraoPorIp", contexto =>
            {
                var ipDoCliente = contexto.Connection.RemoteIpAddress?.ToString() ?? "Desconhecido";
                
                return RateLimitPartition.GetFixedWindowLimiter(ipDoCliente, _ => 
                    new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 20,
                        Window = TimeSpan.FromSeconds(10),
                        QueueLimit = 0
                    });
            });

            options.AddPolicy("RigidaPorIp", contexto =>
            {
                var ipDoCliente = contexto.Connection.RemoteIpAddress?.ToString() ?? "Desconhecido";
                
                return RateLimitPartition.GetFixedWindowLimiter(ipDoCliente, _ => 
                    new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 2,
                        Window = TimeSpan.FromSeconds(10),
                        QueueLimit = 0
                    });
            });

            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
        });

        return services;
    }

}
