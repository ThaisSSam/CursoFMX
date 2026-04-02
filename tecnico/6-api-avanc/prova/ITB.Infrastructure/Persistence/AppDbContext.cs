using Microsoft.EntityFrameworkCore;
using ITB.Domain.Entities;
using ITB.Application.Dtos;

namespace ITB.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Marca> marcas { get; set; }
    public DbSet<Veiculo> veiculos { get; set; }
    public DbSet<Modelo> Modelos { get; set; }
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<MarcaVeiculoFlatDTO> MarcaVeiculoFlats { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 1. Mapeamento da Tabela de Usuários (Fix para Postgres Case-Sensitivity)
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("usuarios"); 
            entity.HasKey(u => u.id);

            // Mapeia as propriedades da Classe (C#) para as Colunas do Banco (Postgres)
            entity.Property(u => u.id).HasColumnName("id");
            entity.Property(u => u.name).HasColumnName("nome");
            entity.Property(u => u.email).HasColumnName("email");
            entity.Property(u => u.senha).HasColumnName("senha");
            entity.Property(u => u.perfil).HasColumnName("perfil");
        });

        // 2. Relacionamentos (Evita a criação de colunas duplicadas como MarcaId1)
        modelBuilder.Entity<Modelo>()
            .HasOne(m => m.Marca)
            .WithMany(ma => ma.Modelos)
            .HasForeignKey(m => m.MarcaId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Veiculo>()
            .HasOne(v => v.Modelo)
            .WithMany(mo => mo.Veiculos)
            .HasForeignKey(v => v.ModeloId)
            .OnDelete(DeleteBehavior.Restrict);

        // 3. Concurrency Token e Preço
        modelBuilder.Entity<Veiculo>(entity => 
        {
            entity.ToTable("veiculos");
            entity.Property(v => v.VersaoLinha).HasColumnName("versao_linha");            
            
            entity.Property(v => v.PrecoCusto).HasColumnName("preco_custo");
            entity.Property(v => v.PrecoVenda).HasColumnName("preco_venda");
        });

        // 4. Configurações de DTO/Views
        modelBuilder.Entity<MarcaVeiculoFlatDTO>(entity => 
        {
            entity.HasNoKey();
            entity.ToView(null); 
        });

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}