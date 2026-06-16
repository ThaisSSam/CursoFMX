using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Treinamento.Domain.Aggregates.Tarefa;

namespace Treinamento.Infrastructure.Persistence.Mappings;

public class TarefaMapping : IEntityTypeConfiguration<Tarefa>
{
    public void Configure(EntityTypeBuilder<Tarefa> builder)
    {
        builder.ToTable("tb_tarefas", "treinamento");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .HasColumnName("Cod")
            .ValueGeneratedOnAdd();

        builder.Property(t => t.Nome)
            .HasColumnType("VARCHAR(150)")
            .IsRequired();

        builder.Property(t => t.Situacao)
            .HasColumnType("INT")
            .IsRequired();

        builder.Property(t => t.Prioridade)
            .HasColumnType("INT")
            .IsRequired();

        builder.Property(t => t.DataCriacao)
            .HasColumnType("TIMESTAMP") 
            .IsRequired();

        builder.HasOne(t => t.UsuarioResponsavel) 
            .WithMany()                        
            .HasForeignKey(t => t.UsuarioId)    
            .OnDelete(DeleteBehavior.Restrict); 
    }
}