using System;

namespace ITB.Domain.Interfaces;

public interface IUnitOfWork
{
    IEnumerable<object> marcas { get; set; }

    Task<bool> CommitAsync();
}
