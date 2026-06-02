using Microsoft.EntityFrameworkCore;
using Treinamento.Domain.Aggregates.Tarefa;
using Treinamento.Domain.Aggregates.Usuarios;

namespace Treinamento.Infrastructure.Persistence;

public abstract class TreinamentoContextBase(DbContextOptions options) : DbContext(options)
{
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Tarefa> Tarefa { get; set; }
}
