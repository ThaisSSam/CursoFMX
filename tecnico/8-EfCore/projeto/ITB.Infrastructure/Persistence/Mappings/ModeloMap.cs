using ITB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ITB.Infrastructure.Persistence.Mappings;

public class ModeloMap : IEntityTypeConfiguration<Modelo>
{
    public void Configure(EntityTypeBuilder<Modelo> builder)
    {
        builder.ToTable("modelos");
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();
        builder.Property(m => m.Nome)
            .HasColumnName("nome")
            .HasColumnType("varchar(100)")
            .IsRequired();
        builder.Property(m => m.MarcaId)
            .HasColumnName("marca_id")
            .HasColumnType("int")
            .IsRequired();
        builder.Property(m => m.Ativo)
            .HasColumnName("ativo")
            .HasDefaultValue(true)
            .IsRequired();

        builder.HasQueryFilter(m => m.Ativo);

        #region Relacionamentos
        builder.HasMany(m => m.Veiculos)
            .WithOne(v => v.Modelo)
            .HasForeignKey(v => v.MarcaId)
            .OnDelete(DeleteBehavior.Restrict);
        #endregion

    }
}
