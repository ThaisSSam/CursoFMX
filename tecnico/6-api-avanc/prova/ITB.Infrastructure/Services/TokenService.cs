using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Collections.Generic;
using ITB.Application.Interfaces;
using ITB.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ITB.Infrastructure.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration; // Se faltar isso, dá Erro 500
    }

    public string GerarToken(Usuario usuario)
    {
        var key = _configuration["JwtSettings:SecretKey"]; // Verifique se o nome no User Secrets é exatamente este
        if (string.IsNullOrEmpty(key))
        {
            // Se cair aqui, o erro 500 é por falta da chave no segredo!
            throw new Exception("Chave JWT não encontrada nas configurações.");
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var keyBytes = Encoding.ASCII.GetBytes(key);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
    {
        // Use ?? para garantir que nunca passe null para os Claims
        new Claim(ClaimTypes.NameIdentifier, usuario.id.ToString()),
        new Claim(ClaimTypes.Name, usuario.name ?? "Usuario"),
        new Claim(ClaimTypes.Email, usuario.email ?? "email@itb.com"),
        new Claim(ClaimTypes.Role, usuario.perfil ?? "Vendedor")
    }),
            Expires = DateTime.UtcNow.AddMinutes(60),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}