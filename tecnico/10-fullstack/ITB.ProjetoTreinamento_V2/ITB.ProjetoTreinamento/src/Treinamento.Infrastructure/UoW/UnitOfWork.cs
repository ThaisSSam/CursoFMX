using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
using Treinamento.Domain.Core.Commands;
using Treinamento.Domain.Core.Interfaces;
using Treinamento.Infrastructure.Persistence;

namespace Treinamento.Infrastructure.UoW;

public class UnitOfWork(TreinamentoWriteContext writeContext, ILogger<UnitOfWork> logger) : IUnitOfWork
{
    private readonly TreinamentoWriteContext _writeContext = writeContext;

    public void Dispose() => _writeContext.Dispose();

    public void ClearChangeTracker() => _writeContext.ChangeTracker.Clear();

    public Task<CommandResponse> SalvarAlteracoesPendentesAsync(CancellationToken cancellationToken = default) =>
        SalvarInternoAsync(cancellationToken);

    public Task<CommandResponse> Commit(CancellationToken cancellationToken = default) =>
        SalvarInternoAsync(cancellationToken);

    private async Task<CommandResponse> SalvarInternoAsync(CancellationToken cancellationToken)
    {
        try
        {
            var rowsAffected = await _writeContext.SaveChangesAsync(cancellationToken);
            logger.LogDebug("SaveChanges executado. LinhasAfetadas={LinhasAfetadas}", rowsAffected);
            return new CommandResponse(true);
        }
        catch (DbUpdateException dbEx) when (dbEx.InnerException is PostgresException pgEx)
        {
            logger.LogError(dbEx, "Erro PostgreSQL. SqlState={SqlState}", pgEx.SqlState);
            return new CommandResponse(false, pgEx.MessageText);
        }
        catch (DbUpdateException dbEx)
        {
            logger.LogError(dbEx, "Erro ao salvar dados.");
            return new CommandResponse(false, dbEx.Message);
        }
    }

    public Task<CommandResponse> CommitInTransactionAsync(Func<CancellationToken, Task> action, CancellationToken cancellationToken = default)
    {
        var strategy = _writeContext.Database.CreateExecutionStrategy();
        return strategy.ExecuteAsync(async ct =>
        {
            await using var transaction = await _writeContext.Database.BeginTransactionAsync(ct);
            try
            {
                await action(ct);
                await _writeContext.SaveChangesAsync(ct);
                await transaction.CommitAsync(ct);
                return new CommandResponse(true);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Falha em commit transacional.");
                return new CommandResponse(false, ex.Message);
            }
        }, cancellationToken);
    }
}
