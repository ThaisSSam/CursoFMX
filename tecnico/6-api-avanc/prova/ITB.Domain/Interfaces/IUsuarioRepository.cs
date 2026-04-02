using ITB.Domain.Entities;

namespace ITB.Domain.Interfaces;

public interface IUsuarioRepository
{
    Task<Usuario?> ObterPorEmailESenha(string email, string senha);
}