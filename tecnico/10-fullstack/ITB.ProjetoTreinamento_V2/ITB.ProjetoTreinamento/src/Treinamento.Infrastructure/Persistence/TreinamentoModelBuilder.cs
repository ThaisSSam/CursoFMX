using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Treinamento.Domain.Core.Validacao;

namespace Treinamento.Infrastructure.Persistence;

internal static class TreinamentoModelBuilder
{
    public static void ApplyShared(ModelBuilder modelBuilder)
    {
        foreach (var property in modelBuilder.Model.GetEntityTypes()
                     .SelectMany(e => e.GetProperties()
                         .Where(p => p.ClrType == typeof(string) && p.GetColumnType() == null)))
        {
            property.SetColumnType("varchar(100)");
        }

        modelBuilder.Ignore<ResultadoValidacaoDominio>();
        modelBuilder.Ignore<ErroValidacaoDominio>();

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
