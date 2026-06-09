using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Treinamento.Domain.Aggregates.Usuarios;

namespace Treinamento.Infrastructure.Mappings;

internal class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("tb_usuarios", "treinamento");

        builder.HasKey(u => u.Id)
            .HasName("pk_tb_usuarios_id");

        builder.Property(u => u.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(u => u.Email)
            .HasColumnName("email")
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(u => u.SenhaHash)
            .HasColumnName("senha_hash")
            .HasMaxLength(512)
            .IsRequired();

        builder.Property(u => u.Ativo)
            .HasColumnName("ativo")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(u => u.TentativasLoginInvalidas)
            .HasColumnName("tentativas_login_invalidas")
            .HasDefaultValue(0)
            .IsRequired();

        builder.Property(u => u.BloqueadoAte)
            .HasColumnName("bloqueado_ate");

        builder.Property(u => u.Nome)
            .HasColumnName("nome")
            .HasMaxLength(100)
            .IsRequired();

        builder.Ignore(u => u.ResultadoValidacao);
    }
}