using ITB.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ITB.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<Marca> Marcas => Set<Marca>();
        public DbSet<Veiculo> Veiculos => Set<Veiculo>();
        public DbSet<Modelo> Modelos => Set<Modelo>();
        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
