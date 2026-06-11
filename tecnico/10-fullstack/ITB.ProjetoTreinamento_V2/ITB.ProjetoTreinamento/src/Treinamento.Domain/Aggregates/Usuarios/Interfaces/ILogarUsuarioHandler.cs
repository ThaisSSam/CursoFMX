using System.Threading.Tasks;
using Treinamento.CrossCutting.Dtos;

namespace Treinamento.Domain.Core.Interfaces;

public interface ILogarUsuarioHandler
{
    Task<(bool Sucesso, string MensagemErro, string? Token)> ExecutarAsync(LoginDto request);
}