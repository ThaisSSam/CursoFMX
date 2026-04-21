using System;
using ITB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ITB.Infrastructure.Persistence.Mappings;

public class UsuarioMapping : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("usuarios");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Nome)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnType("varchar(100)");

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnType("varchar(100)");

        builder.Property(u => u.SenhaHash)
            .IsRequired()
            .HasMaxLength(255) // Tamanho maior prevendo que apliquem Hash (BCrypt) no futuro
            .HasColumnType("varchar(255)");

        builder.Property(u => u.Perfil)
            .IsRequired()
            .HasMaxLength(20)
            .HasColumnType("varchar(20)");
        
        builder.Property(u => u.PrecisaTrocarSenha)
            .HasDefaultValue(false);
            
        // Cria um índice no banco de dados para acelerar o Login
        builder.HasIndex(u => u.Email).IsUnique(); 

        // --- CARGA DE DADOS INICIAL (SEED) ---
        // Dica: Usamos um hash real do BCrypt para que o login funcione no teste
        var senhaHashed = BCrypt.Net.BCrypt.HashPassword("123");

        builder.HasData(
            new 
            { 
                Id = 1, 
                Nome = "Guilherme Paracatu", 
                Email = "admin@itb.com", 
                SenhaHash = senhaHashed, // Agora é um hash de verdade
                Perfil = "Gerente",
                PrecisaTrocarSenha = false 
            },
            new 
            { 
                Id = 2, 
                Nome = "Vendedor de Teste", 
                Email = "vendedor@itb.com", 
                SenhaHash = senhaHashed, 
                Perfil = "Vendedor",
                PrecisaTrocarSenha = false
            }
        );
    }
}
 