using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Treinamento.Domain.Aggregates.Usuarios;
using Treinamento.Domain.Aggregates.Tarefa;
using Treinamento.Domain.Core.Validacao; // 1. Adicione o import das tarefas aqui em cima

namespace Treinamento.Infrastructure.Persistence;

public class TreinamentoReadContext : TreinamentoContextBase
{
    private readonly ILoggerFactory _loggerFactory;
    public DbSet<Tarefa> Tarefas { get; set; }

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
        modelBuilder.Ignore<ErroValidacaoDominio>();
        modelBuilder.Ignore<ResultadoValidacaoDominio>();
        // Mapeamento do Usuário
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("tb_usuarios", "treinamento"); 

            entity.HasKey(u => u.Id);
            entity.Property(u => u.Id).HasColumnName("id");
            entity.Property(u => u.Ativo).HasColumnName("ativo");
            entity.Property(u => u.BloqueadoAte).HasColumnName("bloqueado_ate");
            entity.Property(u => u.Email).HasColumnName("email");
            entity.Property(u => u.SenhaHash).HasColumnName("senha_hash");
            entity.Property(u => u.TentativasLoginInvalidas).HasColumnName("tentativas_login_invalidas");
        });

        // Mapeamento da Tarefa
        modelBuilder.Entity<Tarefa>(entity =>
        {
            entity.ToTable("tb_tarefas", "treinamento"); 
    
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Id).HasColumnName("Cod");
            entity.Property(t => t.Nome).HasColumnType("VARCHAR(150)");
            entity.Property(t => t.Situacao).HasColumnType("INT");
            entity.Property(t => t.Prioridade).HasColumnType("INT");
            entity.Property(t => t.DataCriacao).HasColumnType("TIMESTAMP");
            entity.Property(t => t.UsuarioId).HasColumnType("integer");

            // Relacionamento de leitura
            entity.HasOne(t => t.UsuarioResponsavel)
                .WithMany()
                .HasForeignKey(t => t.UsuarioId);
        });

        base.OnModelCreating(modelBuilder);
    }
}