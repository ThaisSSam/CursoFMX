using System;
using ITB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ITB.Infrastructure.Persistence.Mappings;

public class FabricanteMap : IEntityTypeConfiguration<Fabricante>
{
    public void Configure(EntityTypeBuilder<Fabricante> builder)
    {
        // 1. Nome da Tabela
        builder.ToTable("fabricantes");
        // 2. Chave Primária
        builder.HasKey(f => f.Id);
        // 3. Configuração de Colunas e Tipos (O que você precisa saber)
        builder.Property(f => f.Id)
        .HasColumnName("id")
        .ValueGeneratedOnAdd(); // Garante que o banco gere o ID (Serial/Identity)
        builder.Property(f => f.Nome)
        .HasColumnName("nome")
        .HasColumnType("varchar(100)") // Força o uso de varchar em vez de text
        .IsRequired(); // NOT NULL
    }
}
