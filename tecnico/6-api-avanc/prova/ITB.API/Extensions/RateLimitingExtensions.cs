using System;
using System.Threading.RateLimiting;

namespace ITB.API.Extensions;

public static class RateLimitingExtensions
{
    public static IServiceCollection AddApiRateLimiting(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            // 1. REGRA PADRÃO (Navegação comum: 20 acessos a cada 10s por IP)
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

            // 2. REGRA RÍGIDA (Ataques ou Spam: 2 acessos a cada 10s por IP)
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

            // Customizando a mensagem de erro (Padrão de mercado)
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
        });

        return services;
    }
}
