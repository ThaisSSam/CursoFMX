using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace ITB.API.Extensions;

public static class JwtExtensions
{
  public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
  {
    // 1. Vai lá no nosso cofre (User Secrets) e pega a senha super secreta
    var secretKey = configuration["JwtSettings:SecretKey"];

    // 2. Transforma a senha de texto para um formato de bytes (que a criptografia exige)
    var key = Encoding.ASCII.GetBytes(secretKey!);

    // 3. Avisa o .NET que vamos usar um sistema de Autenticação
    services.AddAuthentication(options =>
    {
      // Define o JWT (Bearer) como o padrão para verificar quem é o usuário
      options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
      options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    // 4. Configura as regras rigorosas do porteiro para aceitar o token
    .AddJwtBearer(options =>
    {
      // Em desenvolvimento deixamos false. Em produção DEVE ser true para exigir HTTPS!
      options.RequireHttpsMetadata = true;

      // Guarda o token na memória para podermos acessar os dados dele depois
      options.SaveToken = true;

      // A lista de verificações que o .NET vai fazer em cada requisição:
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

        // Zera os 5 minutos de tolerância padrão do .NET
        // ClockSkew = TimeSpan.Zero
      };
    });

    return services; // Devolve os serviços configurados
  }
}

