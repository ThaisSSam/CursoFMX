using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ITB.Application.Interfaces;
using ITB.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ITB.Infrastructure.Services;

public class TokenService : ITokenService
{
    IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GerarToken(Usuario usuario)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()), // O ID do usuário no banco (Ex: 99)
            new Claim(JwtRegisteredClaimNames.Email, usuario.Email), // O E-mail
            new Claim(ClaimTypes.Name, usuario.Nome), // O Nome
            new Claim(ClaimTypes.Role, usuario.Perfil) // O Cargo (Isso é o que dá os superpoderes!)
        };

        // 3. Pegando a nossa Chave Secreta do User Secrets para carimbar o token
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]!));

        // 4. Escolhendo o algoritmo de criptografia pesada (HmacSha256)
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // 5. Fabricando fisicamente o Token
        var tokenInfo = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:Issuer"], // Quem emitiu
            audience: _configuration["JwtSettings:Audience"], // Para quem é
            claims: claims, // Os dados do usuário
            expires: DateTime.UtcNow.AddMinutes(int.Parse(_configuration["JwtSettings:ExpirationMinutes"]!)), // Data de validade
            signingCredentials: creds // A assinatura final
        );

        // 6. Transforma o objeto do token em uma String para mandar pela internet
        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenInfo);

        return tokenString;
    }
}
