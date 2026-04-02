using ITB.Domain.Entities;

namespace ITB.Domain.Interfaces;

public interface IUsuarioRepository
{
    Task<Usuario?> ObterPorEmailESenhaAsync(string email, string senha);
}