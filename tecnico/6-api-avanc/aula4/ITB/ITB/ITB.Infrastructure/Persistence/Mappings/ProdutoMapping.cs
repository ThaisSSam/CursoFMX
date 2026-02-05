using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ITB.Domain.Entities;

namespace ITB.Infrastructure.Persistence.Mappings;

public class ProdutoMapping : IEntityTypeConfiguration<Produto>
{
    public void Configure(EntityTypeBuilder<Produto> builder)
    {
        // 1. Nome da Tabela no PostgreSQL
        builder.ToTable("produtos");

        // 2. Chave Primária
        builder.HasKey(p => p.id);

        // 3. Configuração de Colunas (Nomes em minúsculo conforme Dica SOLID)
        builder.Property(p => p.id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(p => p.nome)
            .HasColumnName("nome")
            .HasColumnType("varchar(150)")
            .IsRequired();

        builder.Property(p => p.preco)
            .HasColumnName("preco")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(p => p.fabricanteId)
            .HasColumnName("fabricanteid");

        // 4. Relacionamento (Um Produto tem um Fabricante)
        builder.HasOne(p => p.fabricante)     
           .WithMany()                  
           .HasForeignKey(p => p.fabricanteId) 
           .OnDelete(DeleteBehavior.Restrict);
    }
}