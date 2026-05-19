using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Treinamento.Infrastructure.Persistence;

public class TreinamentoReadContext : TreinamentoContextBase
{
    private readonly ILoggerFactory _loggerFactory;

    public TreinamentoReadContext(DbContextOptions<TreinamentoReadContext> options, ILoggerFactory loggerFactory)
        : base(options)
    {
        _loggerFactory = loggerFactory;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured) return;
        optionsBuilder.UseLoggerFactory(_loggerFactory);
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        TreinamentoModelBuilder.ApplyShared(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }
}
