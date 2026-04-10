using ITB.Domain.Entities;

namespace ITB.Domain.Interfaces;

public interface IMarcaRepository : IRepositoryBase<Marca>
{
    Task<bool> VerificarExistencia(int id);
    Task<bool> PossuiVeiculosAtivos(int marcaId);
}
