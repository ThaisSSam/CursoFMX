using Microsoft.EntityFrameworkCore;
using ITB.Domain.Entities;
using ITB.Application.Dtos;

namespace ITB.Infrastructure.Persistence;
// 2. definir chaves primárias e nomes de tabelas.
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<Marca> marcas { get; set; }
    public DbSet<Veiculo> veiculos{ get; set; }
    public DbSet<Modelo> Modelos { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<MarcaVeiculoFlatDTO> MarcaVeiculoFlats { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 1. Corrigindo as relações (Para sumir o MarcaId1 e ModeloId1)
        modelBuilder.Entity<Modelo>()
            .HasOne(m => m.Marca)
            .WithMany(ma => ma.Modelos) // Certifique-Action: a classe Marca tem a lista 'Modelos'
            .HasForeignKey(m => m.MarcaId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Veiculo>()
            .HasOne(v => v.Modelo)
            .WithMany(mo => mo.Veiculos) // Certifique-Action: a classe Modelo tem a lista 'Veiculos'
            .HasForeignKey(v => v.ModeloId)
            .OnDelete(DeleteBehavior.Restrict);

        // 2. Corrigindo o Concurrecy Token (xmin)
        // Se quiser usar o xmin do Postgres, NÃO use .HasColumnName("xmin")
        // Use apenas .IsRowVersion() ou mude o nome da coluna no banco
        modelBuilder.Entity<Veiculo>()
            .Property(v => v.VersaoLinha)
            .HasColumnName("versao_linha"); // Mude para um nome que não seja xmin            

        // 3. O resto das suas configurações
        modelBuilder.Entity<MarcaVeiculoFlatDTO>(entity => {
            entity.HasNoKey();
            entity.ToView(null); 
        });

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}

// 3. Depois disso vem migration: 
// Adicionar uma migration
// dotnet ef migrations add NomeDaMigration --project ../ITB.Infrastructure --startup-project .

// Atualizar banco 
// dotnet ef database update