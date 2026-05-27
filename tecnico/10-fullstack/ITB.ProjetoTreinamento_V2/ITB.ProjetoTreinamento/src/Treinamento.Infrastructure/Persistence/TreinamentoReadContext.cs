using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Treinamento.Domain.Aggregates.Usuarios;

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
        modelBuilder.Entity<Usuario>(entity =>
        {
            // Força o EF a usar o nome correto da tabela se for minúsculo
            entity.ToTable("Usuarios");

            entity.HasKey(u => u.Id);
            entity.Property(u => u.Id).HasColumnName("id");
            entity.Property(u => u.Ativo).HasColumnName("ativo");
            entity.Property(u => u.BloqueadoAte).HasColumnName("bloqueado_ate");

            // Mapeamentos cruciais para o seu login funcionar:
            entity.Property(u => u.Email).HasColumnName("email");
            entity.Property(u => u.SenhaHash).HasColumnName("senha_hash");
            entity.Property(u => u.TentativasLoginInvalidas).HasColumnName("tentativas_login_invalidas");
        });

        base.OnModelCreating(modelBuilder);
    }
}
