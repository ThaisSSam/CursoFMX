using Microsoft.EntityFrameworkCore;
using ITB.Domain.Entities; 

namespace ITB.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Adicione aqui as tabelas que você quer que o EF crie no Postgres
    public DbSet<Fabricante> fabricantes { get; set; }
}