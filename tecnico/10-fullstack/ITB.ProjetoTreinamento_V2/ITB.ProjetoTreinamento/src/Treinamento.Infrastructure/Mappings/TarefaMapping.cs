using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Treinamento.Domain.Aggregates.Tarefas;

namespace Treinamento.Infrastructure.Persistence.Mappings;

public class TarefaMapping : IEntityTypeConfiguration<Tarefa>
{
    public void Configure(EntityTypeBuilder<Tarefa> builder)
    {
        builder.ToTable("tb_tarefas"); // Nome da tabela no banco

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .HasColumnName("Cod")
            .ValueGeneratedOnAdd();

        builder.Property(t => t.Nome)
            .HasColumnType("VARCHAR(150)")
            .IsRequired();

        // Salva os Enums como INT no Postgres
        builder.Property(t => t.Situacao)
            .HasColumnType("INT")
            .IsRequired();

        builder.Property(t => t.Prioridade)
            .HasColumnType("INT")
            .IsRequired();

        builder.Property(t => t.DataCriacao)
            .HasColumnType("TIMESTAMP") 
            .IsRequired();

        // 2. MAPEAMENTO DA CHAVE ESTRANGEIRA (FK)
        builder.HasOne(t => t.UsuarioResponsavel) // A tarefa tem um usuário responsável
            .WithMany()                           // O usuário pode ter muitas tarefas (ou .WithMany(u => u.Tarefas) se tiver a lista lá)
            .HasForeignKey(t => t.UsuarioId)      // O campo que guarda a chave na tabela de tarefas
            .OnDelete(DeleteBehavior.Restrict);   // Impede de deletar um usuário se ele tiver tarefas pendentes
    }
}