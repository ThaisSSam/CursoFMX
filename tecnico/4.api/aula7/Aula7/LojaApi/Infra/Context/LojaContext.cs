using System;
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
    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<PedidoProduto> PedidoProdutos { get; set; }

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

        // --- MAPEAMENTO DA ENTIDADE PRODUTO ---
        // --- CONFIGURAÇÃO DA ENTIDADE PRODUTO ---
        modelBuilder.Entity<Produto>(entity =>
        {
            // Mapeia para a tabela específica
            // Equivalente a: [Table("TB_PRODUTOS")]
            entity.ToTable("TB_PRODUTOS");

            // Define a Chave Primária
            // Equivalente a: [Key]
            entity.HasKey(p => p.Id);

            // --- PROPRIEDADES ---
            // Configura a coluna Id
            // Equivalente a: [Column("id_produto")]
            entity.Property(p => p.Id)
                  .HasColumnName("id_produto");

            // Configura a coluna Nome
            // Equivalente a: [Column("nome_produto")]
            entity.Property(p => p.Nome)
                  .HasColumnName("nome_produto")
                  .IsRequired() // Boa prática, torna a coluna NOT NULL
                  .HasMaxLength(100); // Boa prática, define um tamanho (ex: 100)

            // Configura a coluna Preco
            // Equivalente a: [Column("preco_produto", TypeName = "decimal(10, 2)")] e [Required]
            entity.Property(p => p.Preco)
                  .HasColumnName("preco_produto")
                  .IsRequired()
                  .HasColumnType("decimal(10, 2)");

            // Configura a coluna Estoque
            // Equivalente a: [Column("estoque_produto")]
            entity.Property(p => p.Estoque)
                  .HasColumnName("estoque_produto");

            // Configura a coluna CategoriaId (FK)
            // Equivalente a: [Column("id_categoria")]
            entity.Property(p => p.CategoriaId)
                  .HasColumnName("id_categoria")
                  .IsRequired();
        });

        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.ToTable("TB_PEDIDOS"); // Especifica o nome correto da tabela
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Id).HasColumnName("id_pedido");
            entity.Property(p => p.DataPedido).HasColumnName("data_pedido");
            entity.Property(p => p.ValorTotal).HasColumnName("valor_total").HasColumnType("decimal(10, 2)");
            entity.Property(p => p.ClienteId).HasColumnName("id_cliente");
        });

        modelBuilder.Entity<PedidoProduto>(entity =>
        {
            entity.ToTable("TB_PEDIDOS_PRODUTOS");
            // Define a Chave Primária Composta
            entity.HasKey(pp => new { pp.PedidoId, pp.ProdutoId });
            // Mapeia as colunas
            entity.Property(pp => pp.PedidoId).HasColumnName("id_pedido");
            entity.Property(pp => pp.ProdutoId).HasColumnName("id_produto");
            entity.Property(pp => pp.Quantidade).HasColumnName("quantidade");
        });

        #region Relacionamentos
        // --- CONFIGURAÇÃO DO RELACIONAMENTO 1:1 ---
        modelBuilder.Entity<Cliente>()
            .HasOne(c => c.Endereco)      // Um Cliente tem um Endereço
            .WithOne(e => e.Cliente)      // Um Endereço tem um Cliente
            .HasForeignKey<Endereco>(e => e.Id); // A chave estrangeira está em Endereco.Id

        // --- CONFIGURAÇÃO DO RELACIONAMENTO 1:N ---
        modelBuilder.Entity<Categoria>()
            .HasMany(c => c.Produtos)
            .WithOne(p => p.Categoria)
            .HasForeignKey(c => c.CategoriaId);

        // 1. Relacionamento 1:N -> Produto (1) : (N) PedidoProdutos
        modelBuilder.Entity<Produto>()
            .HasMany(p => p.PedidoProdutos)
            .WithOne(pp => pp.Produto)
            .HasForeignKey(pp => pp.ProdutoId)
            .HasConstraintName("fk_produto");

        // 2. Relacionamento 1:N -> Pedido (1) : (N) PedidoProdutos
        modelBuilder.Entity<Pedido>()
            .HasMany(p => p.PedidoProdutos)
            .WithOne(pp => pp.Pedido)
            .HasForeignKey(pp => pp.PedidoId)
            .HasConstraintName("fk_pedido");
        #endregion
    }
}
