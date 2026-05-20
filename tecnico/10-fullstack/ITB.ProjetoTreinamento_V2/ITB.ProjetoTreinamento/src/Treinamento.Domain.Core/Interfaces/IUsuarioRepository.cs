using System.Threading.Tasks;
using Treinamento.Domain.Aggregates.Usuarios;

namespace Treinamento.Domain.Aggregates.Usuarios;

public interface IUsuarioRepository
{
    Task<Usuario?> ObterPorEmailAsync(string email);
    Task LogarAsync(Usuario usuario);
}