using System;
using ApiEstoque.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiEstoque.Infra.Context;

public class LojaDbContext : DbContext {
    public LojaDbContext(DbContextOptions<LojaDbContext> options) : base(options) { }
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<Fabricante> Fabricantes { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Produto>(entity =>
        {
            entity.ToTable("produtos");

            entity.HasKey(p => p.Id);
            entity.Property(p => p.Id).HasColumnName("id");

            entity.Property(p => p.Nome).HasColumnName("nome").HasMaxLength(100).IsRequired();
            entity.Property(p => p.Preco).HasColumnName("preco").IsRequired();
            entity.Property(p => p.FabricanteId).HasColumnName("fabricanteid").IsRequired();
        });
        modelBuilder.Entity<Fabricante>(entity =>
        {
            entity.ToTable("fabricantes");

            entity.HasKey(f => f.Id);
            entity.Property(f => f.Id).HasColumnName("id");
            entity.Property(f => f.Nome).HasColumnName("nome").HasMaxLength(100).IsRequired();

        });
/*
        modelBuilder.Entity<Fabricante>()
            .HasMany(f => f.Produtos)
            .WithOne(p => p.Fabricante)
            
            .HasForeignKey(f => f.FabricanteId);
*/
        modelBuilder.Entity<Produto>()
            .HasOne(f => f.Fabricante)
            .WithMany(p => p.Produtos)
            
            .HasForeignKey(f => f.FabricanteId);
            
    }
}
