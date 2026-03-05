using System;
using ITB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ITB.Infrastructure.Persistence.Mappings;

public class ModeloMapping : IEntityTypeConfiguration<Modelo>
{
    public void Configure(EntityTypeBuilder<Modelo> builder)
    {
        builder.ToTable("modelos");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Nome).IsRequired().HasMaxLength(100);

        builder.HasOne(m => m.Marca) 
        .WithMany()
        .HasForeignKey(m => m.MarcaId)
        .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(x => x.Ativo);
    }
}
