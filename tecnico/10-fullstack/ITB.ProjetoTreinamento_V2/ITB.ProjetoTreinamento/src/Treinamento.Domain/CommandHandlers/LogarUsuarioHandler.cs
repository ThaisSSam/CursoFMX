using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Treinamento.Domain.Aggregates.Usuarios;
using Treinamento.Domain.Core.Interfaces;
using Treinamento.CrossCutting.Dtos;

namespace Treinamento.Domain.Handlers;

public class LogarUsuarioHandler : ILogarUsuarioHandler
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IConfiguration _configuration;

    public LogarUsuarioHandler(IUsuarioRepository usuarioRepository, IConfiguration configuration)
    {
        _usuarioRepository = usuarioRepository;
        _configuration = configuration;
    }

    public async Task<(bool Sucesso, string MensagemErro, string? Token)> ExecutarAsync(LoginDto request)
    {
        var usuario = await _usuarioRepository.ObterPorEmailAsync(request.Email);

        if (usuario == null)
        {
            return (false, "E-mail ou senha incorretos. Verifique suas credenciais e tente novamente.", null);
        }

        bool loginValido = usuario.RealizarTentativaLogin(request.Senha);
        if (!loginValido)
        {
            await _usuarioRepository.LogarAsync(usuario);
            return (false, usuario.ResultadoValidacao.Erros[0].MensagemErro, null);
        }
        
        await _usuarioRepository.LogarAsync(usuario);

        string tokenGerado = GerarJwtToken(usuario);

        return (true, string.Empty, tokenGerado);
    }

    private string GerarJwtToken(Usuario usuario)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var secretKey = _configuration.GetSection("JwtSettings:Secret").Value
            ?? throw new InvalidOperationException("Chave secreta do JWT não configurada no User Secrets.");

        var issuer = _configuration.GetSection("JwtSettings:Issuer").Value;
        var audience = _configuration.GetSection("JwtSettings:Audience").Value;

        var chaveCriptografada = Encoding.ASCII.GetBytes(secretKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email)
            }),
            Expires = DateTime.UtcNow.AddHours(2),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(chaveCriptografada),
                SecurityAlgorithms.HmacSha256Signature
            )
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}