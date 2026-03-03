using System;
using ITB.Domain.Entities;

namespace ITB.Domain.Interfaces;

public interface IModeloRepository : IRepositoryBase<Modelo>
{
    // Task AdicionarAsync(Modelo modelo);

    // // Task Atualizar(Modelo modelo);
    // Task<IEnumerable<Modelo>> ObterTodos();
    // Task<Modelo?> ObterPorIdAsync(int id);    

    Task<bool> VerificarExistencia(int id);
    Task<bool> PossuiVeiculosAtivos(int modeloId);

}
