using ITB.Domain.Entities;

namespace ITB.Domain.Interfaces;

public interface IUsuarioRepository : IRepositoryBase<Usuario>
{
    Task<Usuario?> ObterPorEmailAsync(string email);
}
