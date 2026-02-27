using ITB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ITB.Infrastructure.Persistence.Mappings;

public class VeiculoMapping : IEntityTypeConfiguration<Veiculo>
{
    public void Configure(EntityTypeBuilder<Veiculo> builder)
    {
        // Define o nome da tabela
        builder.ToTable("Veiculos");

        // Chave Primária
        builder.HasKey(v => v.Id);

        // // Configuração das Propriedades
        // builder.Property(v => v.Modelo)
        //     .IsRequired()
        //     .HasMaxLength(100)
        //     .HasColumnType("varchar(100)");

        builder.Property(v => v.Placa)
            .IsRequired()
            .HasMaxLength(7)
            .HasColumnType("char(7)");

        builder.Property(v => v.Ano)
            .IsRequired();

        // Configuração do Relacionamento (Chave Estrangeira)
        // Assumindo que você tem uma entidade Marca
        builder.Property(v => v.ModeloId)
            .IsRequired();

        // builder.HasOne<Marca>() // Se houver a propriedade de navegação: builder.HasOne(v => v.Marca)
        //     .WithMany()         // Se a Marca tiver lista de Veículos: .WithMany(m => m.Veiculos)
        //     .HasForeignKey(v => v.ModeloId)
        //     .OnDelete(DeleteBehavior.Restrict);

        // Índice para a Placa (Geralmente placas são únicas)
        builder.HasIndex(v => v.Placa)
            .IsUnique();

        builder.HasOne(v => v.Modelo)
           .WithMany() // ou .WithMany(m => m.Veiculos) se houver a lista na Marca
           .HasForeignKey(v => v.ModeloId) // <-- O segredo está aqui
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasQueryFilter(x => x.Ativo);
    }
}