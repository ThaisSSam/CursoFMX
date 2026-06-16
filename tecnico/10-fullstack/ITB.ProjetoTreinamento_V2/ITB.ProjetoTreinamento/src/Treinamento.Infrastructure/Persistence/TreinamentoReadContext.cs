using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Treinamento.Domain.Aggregates.Usuarios;
using Treinamento.Domain.Aggregates.Tarefa;
using Treinamento.Domain.Core.Validacao;

namespace Treinamento.Infrastructure.Persistence;

public class TreinamentoReadContext : TreinamentoContextBase
{
    private readonly ILoggerFactory _loggerFactory;
    public DbSet<Tarefa> Tarefas { get; set; }
    public DbSet<TarefaHistorico> TarefasHistorico { get; set; } 

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

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("tb_usuarios", "treinamento"); 

            entity.HasKey(u => u.Id);
            entity.Property(u => u.Id).HasColumnName("id");
            entity.Property(u => u.Ativo).HasColumnName("ativo");
            entity.Property(u => u.BloqueadoAte).HasColumnName("bloqueado_ate");
            entity.Property(u => u.Nome).HasColumnName("nome");
            entity.Property(u => u.Email).HasColumnName("email").IsRequired();
            entity.Property(u => u.SenhaHash).HasColumnName("senha_hash").IsRequired();
            entity.Property(u => u.TentativasLoginInvalidas).HasColumnName("tentativas_login_invalidas");
        });

        modelBuilder.Entity<Tarefa>(entity =>
        {
            entity.ToTable("tb_tarefas", "treinamento"); 
    
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Id).HasColumnName("Cod");
            entity.Property(t => t.Nome).HasColumnType("VARCHAR(150)");
            entity.Property(t => t.Situacao).HasColumnType("INT");
            entity.Property(t => t.Prioridade).HasColumnType("INT");
            entity.Property(t => t.DataCriacao).HasColumnType("timestamp with time zone");
            entity.Property(t => t.UsuarioId).HasColumnType("integer");
            entity.Property(t => t.Excluido).HasColumnName("Excluido").HasColumnType("boolean");
            entity.Property(t => t.DataExclusao).HasColumnName("DataExclusao").HasColumnType("timestamp with time zone");

            entity.HasQueryFilter(t => !t.Excluido);

            entity.HasOne(t => t.UsuarioResponsavel)
                .WithMany()
                .HasForeignKey(t => t.UsuarioId);        
        });

        modelBuilder.Entity<TarefaHistorico>(entity =>
        {
            entity.ToTable("tb_tarefas_historico", "treinamento");

            entity.HasKey(h => h.Id);
            entity.Property(h => h.Id).HasColumnName("Id");
            entity.Property(h => h.TarefaId).HasColumnName("TarefaId").HasColumnType("integer");
            entity.Property(h => h.Nome).HasColumnName("Nome").HasColumnType("VARCHAR(255)");
            entity.Property(h => h.Situacao).HasColumnName("Situacao").HasColumnType("integer");
            entity.Property(h => h.Prioridade).HasColumnName("Prioridade").HasColumnType("integer");
            entity.Property(h => h.UsuarioId).HasColumnName("UsuarioId").HasColumnType("integer");
            entity.Property(h => h.DataAlteracao).HasColumnName("DataAlteracao").HasColumnType("timestamp with time zone");
            entity.Property(h => h.TipoAcao).HasColumnName("TipoAcao").HasColumnType("VARCHAR(20)");
            entity.Property(h => h.UsuarioAlteracaoId).HasColumnName("UsuarioAlteracaoId").HasColumnType("integer");
        });

        base.OnModelCreating(modelBuilder);
    }
}