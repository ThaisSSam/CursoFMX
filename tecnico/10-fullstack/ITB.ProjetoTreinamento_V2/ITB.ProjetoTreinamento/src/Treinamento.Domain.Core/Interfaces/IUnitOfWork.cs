using Treinamento.Domain.Core.Commands;

namespace Treinamento.Domain.Core.Interfaces;

public interface IUnitOfWork : IDisposable
{
    void ClearChangeTracker();
    Task<CommandResponse> SalvarAlteracoesPendentesAsync(CancellationToken cancellationToken = default);
    Task<CommandResponse> Commit(CancellationToken cancellationToken = default);
    Task<CommandResponse> CommitInTransactionAsync(Func<CancellationToken, Task> action, CancellationToken cancellationToken = default);
}
