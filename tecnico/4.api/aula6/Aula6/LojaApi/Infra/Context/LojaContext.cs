using System;
using System.Runtime.InteropServices;
using LojaApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace LojaApi.Infra.Context;

public class LojaContext : DbContext
{
    public LojaContext(DbContextOptions<LojaContext> options) : base(options) { }

    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<Endereco> Enderecos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // --- MAPEAMENTO DA ENTIDADE CLIENTE ---
        modelBuilder.Entity<Cliente>(entity =>
        {
            // Mapeia para a tabela TB_CLIENTES
            entity.ToTable("TB_CLIENTES");

            // Define a chave primária e o nome da coluna
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).HasColumnName("id_cliente");
            
            // Mapeia as outras propriedades para suas colunas
            entity.Property(c => c.Nome).HasColumnName("nome_cliente").HasMaxLength(150).IsRequired();
            entity.Property(c => c.Email).HasColumnName("email_cliente").HasMaxLength(150).IsRequired();
            entity.Property(c => c.Ativo).HasColumnName("ativo");
            entity.Property(c => c.DataCadastro).HasColumnName("data_cadastro");
        });

        // --- MAPEAMENTO DA ENTIDADE ENDERECO ---
        modelBuilder.Entity<Endereco>(entity =>
        {
            entity.ToTable("TB_ENDERECOS");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id_cliente");
            entity.Property(e => e.Rua).HasColumnName("rua").HasMaxLength(200).IsRequired();
            entity.Property(e => e.Cidade).HasColumnName("cidade").HasMaxLength(100).IsRequired();
            entity.Property(e => e.Estado).HasColumnName("estado").HasMaxLength(50).IsRequired();
            entity.Property(e => e.Cep).HasColumnName("cep").HasMaxLength(10).IsRequired();
        });

        // MAPEAMENTO DA ENTIDADE PRODUTO
        modelBuilder.Entity<Produto>(entity =>
        {
            entity.ToTable("TB_PRODUTOS");
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Id).HasColumnName("id_produto");

            entity.Property(p => p.Nome).HasColumnName("nome_produto").HasMaxLength(150).IsRequired();

            entity.Property(p => p.Preco).HasColumnName("preco_produto").IsRequired();

            entity.Property(p => p.Estoque).HasColumnName("estoque_produto").HasMaxLength(999).IsRequired();

            entity.Property(p => p.CategoriaId).HasColumnName("id_categoria").IsRequired();
        });

        // --- CONFIGURAÇÃO DO RELACIONAMENTO 1:1 ---
        modelBuilder.Entity<Cliente>()
            .HasOne(c => c.Endereco)      // Um Cliente tem um Endereço
            .WithOne(e => e.Cliente)      // Um Endereço tem um Cliente
            .HasForeignKey<Endereco>(e => e.Id); // A chave estrangeira está em Endereco.Id

        modelBuilder.Entity<Categoria>()
            .HasMany(c => c.Produtos)
            .WithOne(p => p.Categoria)
            .HasForeignKey(c => c.CategoriaId);

        base.OnModelCreating(modelBuilder);
    }
}
