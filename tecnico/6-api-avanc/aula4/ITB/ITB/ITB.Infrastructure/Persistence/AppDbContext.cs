using ITB.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ITB.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<Fabricante> Fabricantes => Set<Fabricante>();
        public DbSet<Categoria> Categorias => Set<Categoria>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Produto> Produtos { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Aplica todas as configurações de nomes de tabela e colunas (FluentAPI)
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }

        


    }
}
