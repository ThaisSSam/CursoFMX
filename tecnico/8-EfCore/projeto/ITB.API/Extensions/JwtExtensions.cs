using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace ITB.API.Extensions;

public static class JwtExtensions
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var secretKey = configuration["JwtSettings:SecretKey"];

        var key = Encoding.ASCII.GetBytes(secretKey!);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })

        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = true;
            options.SaveToken = true;

            options.TokenValidationParameters = new TokenValidationParameters
            {
                // A) Exige que o token tenha uma assinatura matemática válida
                ValidateIssuerSigningKey = true,
                
                // B) Entrega a nossa chave secreta para o .NET poder conferir a assinatura
                IssuerSigningKey = new SymmetricSecurityKey(key),
                
                // C) Verifica se fomos nós mesmos que emitimos o token (Issuer)
                ValidateIssuer = true, 
                ValidIssuer = configuration["JwtSettings:Issuer"], 
                
                // D) Verifica se o token está sendo usado no lugar certo (Audience)
                ValidateAudience = true,
                ValidAudience = configuration["JwtSettings:Audience"], 
                
                // E) Verifica o relógio: rejeita na hora se o token já estiver vencido
                ValidateLifetime = true,

                ClockSkew = TimeSpan.Zero
            };
        });
        return services;
    }
}
