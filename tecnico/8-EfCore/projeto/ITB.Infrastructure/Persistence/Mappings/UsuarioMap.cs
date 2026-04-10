using ITB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ITB.Infrastructure.Persistence.Mappings;

public class UsuarioMap : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("usuarios");
        builder.HasKey(v => v.Id);
        builder.Property(v => v.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();
        builder.Property(v => v.Nome)
            .HasColumnName("nome")
            .HasColumnType("varchar(100)")
            .IsRequired();
        builder.Property(v => v.Email)
            .HasColumnName("email")
            .HasColumnType("varchar(100)")
            .IsRequired();
        builder.Property(v => v.SenhaHash)
            .HasColumnName("senha")
            .HasColumnType("varchar(500)")
            .IsRequired();
        builder.Property(v => v.Perfil)
            .HasColumnName("perfil")
            .HasColumnType("varchar(100)")
            .HasDefaultValue(true)
            .IsRequired();
        // builder.Property(v => v._VersaoLinha)
        //     .IsRowVersion();
        builder.Property(v => v.PrecisaTrocarSenha)            
            .HasColumnType("boolean")
            .HasDefaultValue(false);
    }
}
