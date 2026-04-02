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
        _configuration = configuration;
    }

    public string GerarToken(Usuario usuario)
    {
        var secretKey = _configuration["JwtSettings:SecretKey"];
        var key = Encoding.ASCII.GetBytes(secretKey!);

        // Use o System.Security.Claims explicitamente para evitar o erro do BinaryReader
        var claims = new List<System.Security.Claims.Claim>
            {
                new System.Security.Claims.Claim("sub", usuario.id.ToString()),
                new System.Security.Claims.Claim("email", usuario.email),
                new System.Security.Claims.Claim("unique_name", usuario.name),
                new System.Security.Claims.Claim("role", usuario.perfil)
            };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["JwtSettings:ExpirationMinutes"]!)),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = _configuration["JwtSettings:Issuer"],
            Audience = _configuration["JwtSettings:Audience"]
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}