using System;
using System.Threading.Tasks;
using ITB.Domain.Entities;

namespace ITB.Domain.Interfaces;

public interface IMarcaRepository : IRepositoryBase<Marca>
{
    Task<bool> VerificarExistencia(int id); // Mantenha apenas este
    Task<bool> PossuiModelosAtivos(int marcaId);
}