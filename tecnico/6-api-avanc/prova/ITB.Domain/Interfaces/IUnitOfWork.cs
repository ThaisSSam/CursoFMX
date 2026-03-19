using System;

namespace ITB.Domain.Interfaces;

public interface IUnitOfWork
{

    IVeiculoRepository veiculos { get; } // Se houver veículos
    IMarcaRepository marcas { get; }      // O erro aponta que este cara falta!
    Task<bool> CommitAsync();
}
