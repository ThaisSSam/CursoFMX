using System;
using Microsoft.EntityFrameworkCore;

namespace BulkInsertDemo;

public class AppDbContext : DbContext
{
    public DbSet<OperacaoFinanceira> Operacoes { get; set; }
    public DbSet<Veiculo> Veiculos { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Veiculo>().HasIndex(v => v.Placa).IsUnique();
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Database=DbBulkDemo;Username=postgres;Password=postgres");
    }
}
