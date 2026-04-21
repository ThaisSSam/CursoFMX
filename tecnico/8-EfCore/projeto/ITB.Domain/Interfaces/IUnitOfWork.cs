namespace ITB.Domain.Interfaces;

public interface IUnitOfWork
{
    Task<bool> Commit();
}
