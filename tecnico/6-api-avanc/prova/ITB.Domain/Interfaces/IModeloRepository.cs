using System;
using ITB.Domain.Entities;

namespace ITB.Domain.Interfaces;

public interface IModeloRepository
{
    Task AdicionarAsync(Modelo modelo);

    // Task Atualizar(Modelo modelo);
    Task<IEnumerable<Modelo>> ObterTodos();
    Task<Modelo?> ObterPorIdAsync(int id);    
}
