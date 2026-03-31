using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ITB.API.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ITB.API.Controller;

[ApiController]
    [Route("api/[controller]")]
    public class AutenticacaoController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AutenticacaoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        [AllowAnonymous] // Passagem livre do Porteiro do JWT...
        //[ServiceFilter(typeof(ApiKeyAttribute))] // ...mas barrado pela catraca da API Key!
        [ApiKey]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // 1. Validação "Fake" temporária (No mundo real, validamos no Banco de Dados!)
            // FALTA DTO
            if (request.Email != "admin@itb.com" || request.Senha != "123456")
            {
                return Unauthorized(new { mensagem = "Usuário ou senha incorretos." });
            }

            // 2. Montando as Claims (O Payload / O Miolo do Token)
            // São as informações que a API vai "lembrar" sobre o usuário.
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, "99"), // O ID do usuário no banco (Ex: 99)
                new Claim(JwtRegisteredClaimNames.Email, request.Email), // O E-mail
                new Claim(ClaimTypes.Name, "Guilherme Admin"), // O Nome
                new Claim(ClaimTypes.Role, "Gerente") // O Cargo (Isso é o que dá os superpoderes!)
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

            return Ok(new
            {
                Token = tokenString,
                Mensagem = "Bem-vindo! Guarde este token e envie no Header das próximas requisições."
            });
        }
    }

// DTO TEMPORÁRIO
    public class LoginRequest 
{ 
    public string Email { get; set; } = string.Empty; 
    public string Senha { get; set; } = string.Empty; 
} 