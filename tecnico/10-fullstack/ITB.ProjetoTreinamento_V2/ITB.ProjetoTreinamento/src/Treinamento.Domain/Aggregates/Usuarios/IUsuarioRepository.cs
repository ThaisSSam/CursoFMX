using System.Threading.Tasks;

namespace Treinamento.Domain.Aggregates.Usuarios;

public interface IUsuarioRepository
{
    Task<Usuario?> ObterPorEmailAsync(string email);
    Task AtualizarAsync(Usuario usuario);
}