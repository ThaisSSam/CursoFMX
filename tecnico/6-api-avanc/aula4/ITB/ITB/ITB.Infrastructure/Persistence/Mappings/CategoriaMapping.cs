using System;
using ITB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ITB.Infrastructure.Persistence.Mappings;

public class CategoriaMap : IEntityTypeConfiguration<Categoria>
{
    public void Configure(EntityTypeBuilder<Categoria> builder)
    {
        builder.ToTable("categorias");
        builder.HasKey(c=>c.Id);

        builder.Property(c => c.Id)
        .HasColumnName("id")
        .ValueGeneratedOnAdd(); // Garante que o banco gere o ID (Serial/Identity)
        builder.Property(c => c.Nome)
        .HasColumnName("nome")
        .HasColumnType("varchar(100)") // Força o uso de varchar em vez de text
        .IsRequired(); // NOT NULL
    }
}
