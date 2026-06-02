using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Treinamento.Domain.Aggregates.Tarefa;
using Treinamento.Domain.Core.Validacao;

namespace Treinamento.Infrastructure.Persistence;

public class TreinamentoContext(DbContextOptions<TreinamentoWriteContext> options, ILoggerFactory loggerFactory)
    : TreinamentoWriteContext(options, loggerFactory);

public class TreinamentoWriteContext : TreinamentoContextBase
{
    private readonly ILoggerFactory _loggerFactory;

    public DbSet<Tarefa> Tarefas { get; set; }

    public TreinamentoWriteContext(DbContextOptions<TreinamentoWriteContext> options, ILoggerFactory loggerFactory)
        : base(options)
    {
        _loggerFactory = loggerFactory;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured) return;
        optionsBuilder.UseLoggerFactory(_loggerFactory);
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<ErroValidacaoDominio>();
        modelBuilder.Ignore<ResultadoValidacaoDominio>();
        TreinamentoModelBuilder.ApplyShared(modelBuilder);
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TreinamentoContext).Assembly);
    }
}
