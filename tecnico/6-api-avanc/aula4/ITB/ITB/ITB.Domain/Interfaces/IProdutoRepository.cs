using System;
using ITB.Domain.Entities;

namespace ITB.Domain.Interfaces;

public interface IProdutoRepository
{
    Task<IEnumerable<Produto>> ObterTodosAsync();
    Task<Produto?> ObterPorIdAsync(int id);
    
    // ADICIONE ESTES MÉTODOS:
    Task<Produto> AdicionarAsync(Produto produto);
    Task<bool> AtualizarAsync(Produto produto);
    Task<bool> DeletarAsync(Produto produto);
    Task<bool> QualquerProdutoComFabricante(int fabricanteId);
}

// Códigos pro terminal para fazer migration

// Instaladores
// dotnet ef migrations add CriarTabelaFabricante --project ITB.Infrastructure --startup-project ITB.API
 
// dotnet remove ITB.Infrastructure package Microsoft.EntityFrameworkCore
 
// dotnet remove ITB.Infrastructure package Npgsql.EntityFrameworkCore.PostgreSQL
 
// dotnet add ITB.Infrastructure package Microsoft.EntityFrameworkCore --version 8.0.0
 
// dotnet add ITB.Infrastructure package Npgsql.EntityFrameworkCore.PostgreSQL --version 8.0.0
 
// dotnet add ITB.Infrastructure package Microsoft.EntityFrameworkCore.Relational --version 8.0.0
 
// dotnet add ITB.API package Microsoft.EntityFrameworkCore.Design --version 8.0.0
 
// dotnet tool install --global dotnet-ef
 
// dotnet clean
 
// dotnet restore
 
// dotnet build
 
//  Códigos para migration
// dotnet ef database update --project ITB.Infrastructure --startup-project ITB.API
 
// dotnet ef migrations remove --project ITB.Infrastructure --startup-project ITB.API
 
// dotnet ef database update <Nome_Da_Migration_Anterior> --project ITB.Infrastructure --startup-project ITB.API
 