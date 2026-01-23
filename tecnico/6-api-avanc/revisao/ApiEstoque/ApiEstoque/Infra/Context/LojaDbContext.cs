using System;
using ApiEstoque.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiEstoque.Infra.Context;

public class LojaDbContext : DbContext {
    public LojaDbContext(DbContextOptions<LojaDbContext> options) : base(options) { }
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<Fabricante> Fabricantes { get; set; }
    // Onde está a Fluent API aqui?
}
