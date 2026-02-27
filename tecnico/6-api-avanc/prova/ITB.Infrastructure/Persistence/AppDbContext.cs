using Microsoft.EntityFrameworkCore;
using ITB.Domain.Entities; 

namespace ITB.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<Marca> marcas { get; set; }
    public DbSet<Veiculo> veiculos{ get; set; }
    public DbSet<Modelo> modelos{ get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Marca>().ToTable("marcas");
        modelBuilder.Entity<Veiculo>().ToTable("veiculos");
        modelBuilder.Entity<Modelo>().ToTable("modelos");

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}