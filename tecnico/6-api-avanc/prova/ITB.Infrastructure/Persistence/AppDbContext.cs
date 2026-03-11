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
    public DbSet<MarcaVeiculoFlatDTO> MarcaVeiculoFlats { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Marca>().ToTable("marcas");
        modelBuilder.Entity<Veiculo>().ToTable("veiculos");
        modelBuilder.Entity<Modelo>().ToTable("modelos");

        modelBuilder.Entity<MarcaVeiculoFlatDTO>(entity => 
        {
            entity.HasNoKey();
            // .ToView(null) garante que o Migrations não tente criar uma tabela para isso
            entity.ToView(null); 
        });

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}

// 3. Depois disso vem migration: 
// Adicionar uma migration
// dotnet ef migrations add NomeDaMigration

// Atualizar banco 
// dotnet ef database update