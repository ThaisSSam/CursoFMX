using GerenciadorTarefasApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace GerenciadorTarefasApi.Infra.Context
{
    public class TarefaContext : DbContext
    {
        public TarefaContext(DbContextOptions<TarefaContext> options) : base(options) { }
        
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Tarefa> Tarefas { get; set; }
        public DbSet<DetalhesTarefa> DetalhesTarefas { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TarefaTag> TarefasTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("TB_USUARIOS");
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Id).HasColumnName("id_usuario");
                entity.Property(u => u.Nome).IsRequired().HasMaxLength(100).HasColumnName("nome_usuario");
                entity.Property(u => u.Email).IsRequired().HasMaxLength(150).HasColumnName("email_usuario");
                entity.HasIndex(u => u.Email).IsUnique();
            });

    
            modelBuilder.Entity<Tarefa>(entity =>
            {
                entity.ToTable("TB_TAREFAS");
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Id).HasColumnName("id_tarefa");
                entity.Property(t => t.Titulo).IsRequired().HasMaxLength(100).HasColumnName("titulo_tarefa");
                entity.Property(t => t.Descricao).HasMaxLength(500).HasColumnName("descricao_tarefa");
                entity.Property(t => t.DataCriacao).IsRequired().HasColumnName("data_criacao");
                entity.Property(t => t.DataConclusao).HasColumnName("data_conclusao");
                entity.Property(t => t.Concluida).IsRequired().HasDefaultValue(false).HasColumnName("concluida");
                entity.Property(t => t.UsuarioId).IsRequired().HasColumnName("id_usuario");
            });
          
            modelBuilder.Entity<DetalhesTarefa>(entity =>
            {
                entity.ToTable("TB_DETALHES_TAREFA");
                entity.HasKey(d => d.TarefaId);
                entity.Property(d => d.TarefaId).HasColumnName("id_tarefa");
                entity.Property(d => d.Prioridade).IsRequired().HasDefaultValue(0).HasColumnName("prioridade");
                entity.Property(d => d.NotasAdicionais).HasColumnType("TEXT").HasColumnName("notas_adicionais");
            });


           
            modelBuilder.Entity<Tag>(entity =>
            {
                entity.ToTable("TB_TAGS");
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Id).HasColumnName("id_tag");
                entity.Property(t => t.Nome).IsRequired().HasMaxLength(50).HasColumnName("nome_tag");
                entity.HasIndex(t => t.Nome).IsUnique();
            });


            modelBuilder.Entity<TarefaTag>(entity =>
            {
                entity.ToTable("TB_TAREFAS_TAGS");
                entity.HasKey(tt => new { tt.TarefaId, tt.TagId });
                entity.Property(tt => tt.TarefaId).HasColumnName("id_tarefa");
                entity.Property(tt => tt.TagId).HasColumnName("id_tag");
            });


            #region Relacionamentos

            modelBuilder.Entity<Tarefa>()
                      .HasOne(t => t.Usuario)
                      .WithMany(u => u.Tarefas)
                      .HasForeignKey(t => t.UsuarioId)
                      .HasConstraintName("fk_usuario_tarefa");
          
            modelBuilder.Entity<DetalhesTarefa>()
                      .HasOne(d => d.Tarefa)
                      .WithOne(t => t.Detalhes)
                      .HasForeignKey<DetalhesTarefa>(d => d.TarefaId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("fk_tarefa_detalhes");
 
            modelBuilder.Entity<TarefaTag>()
                      .HasOne(tt => tt.Tarefa)
                      .WithMany(t => t.TarefasTags)
                      .HasForeignKey(tt => tt.TarefaId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("fk_tarefatag_tarefa");

            modelBuilder.Entity<TarefaTag>()
                      .HasOne(tt => tt.Tag)
                      .WithMany(tg => tg.TarefasTags)
                      .HasForeignKey(tt => tt.TagId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("fk_tarefatag_tag");
        #endregion

    
        }
    }
}