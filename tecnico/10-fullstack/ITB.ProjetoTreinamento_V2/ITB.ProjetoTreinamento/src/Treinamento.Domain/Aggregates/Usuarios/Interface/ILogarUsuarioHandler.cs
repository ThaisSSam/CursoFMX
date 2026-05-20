using System.Threading.Tasks;
using Treinamento.Domain.Aggregates.Usuarios;

namespace Treinamento.Domain.Core.Interfaces;

public interface ILogarUsuarioHandler
{
    Task<(bool Sucesso, string MensagemErro)> ExecutarAsync(LoginDto request);
}