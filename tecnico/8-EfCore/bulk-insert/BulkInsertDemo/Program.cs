using BulkInsertDemo;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using var context = new AppDbContext();
await context.Database.EnsureCreatedAsync(); // Cria o banco apenas para o teste
// Vamos simular a leitura de um arquivo com 100 MIL registros!
Console.WriteLine("Gerando 100.000 registros na memória...");
var loteImportacao = new List<OperacaoFinanceira>();
for (int i = 0; i < 100000; i++)
{
    loteImportacao.Add(new OperacaoFinanceira
    {
        Id = Guid.NewGuid(),
        NumeroContrato = $"CTR-{i:D6}",
        ValorTotal = new Random().Next(1000, 50000),
        DataProcessamento = DateTime.UtcNow
    });
}
Console.WriteLine("Iniciando operações em Bulk...");
// ==========================================
// RECURSO 1: BulkInsertAsync
// ==========================================
// Insere milhares de registros ignorando o rastreamento em memória.
// Velocidade absurda (geralmente menos de 2 segundos para 100k registros).
Console.WriteLine("1. Testando Bulk Insert...");
await context.BulkInsertAsync(loteImportacao);

// ==========================================
// RECURSO 2: BulkUpdateAsync
// ==========================================
// Se você alterou uma lista gigante na memória e precisa salvar tudo de volta:
Console.WriteLine("2. Testando Bulk Update...");
foreach (var op in loteImportacao)
{
    op.ValorTotal *= 1.1m; // Simulando um reajuste de 10% na memória
}
await context.BulkUpdateAsync(loteImportacao);
// ==========================================
// RECURSO 3: BulkInsertOrUpdateAsync (O Famoso UPSERT)
// ==========================================
// Este é o mais usado em integrações de sistemas!
// Ele verifica: Se a chave primária já existe no banco, faz UPDATE. Se não existe, faz 
Console.WriteLine("3. Testando Bulk Insert Or Update (Upsert)...");
// Adicionando um registro novo à lista que já existe
loteImportacao.Add(new OperacaoFinanceira
{
    Id = Guid.NewGuid(),
    NumeroContrato = "CTR-NOVO",
    ValorTotal = 99999
});
await context.BulkInsertOrUpdateAsync(loteImportacao);
// ==========================================
// RECURSO 4: BulkDeleteAsync
// ==========================================
// Deleta fisicamente todos os registros da lista passada.
Console.WriteLine("4. Testando Bulk Delete...");
await context.BulkDeleteAsync(loteImportacao);
Console.WriteLine("Testes finalizados com sucesso!");

var config = new BulkConfig
{
    BatchSize = 10000, // Envia de 10 em 10 mil para o banco
    CalculateStats = true, // Retorna estatísticas precisas no final
    SetOutputIdentity = false // (Para performance) Avisa que não precisamos que o EF 
};
await context.BulkInsertAsync(loteImportacao, config);
// Lendo as estatísticas do que acabou de acontecer
Console.WriteLine($"Inseridos: {config.StatsInfo?.StatsNumberInserted ?? 0}");
Console.WriteLine($"Atualizados: {config.StatsInfo?.StatsNumberUpdated ?? 0}");

// using BulkInsertDemo;
// using EFCore.BulkExtensions;
// using Microsoft.EntityFrameworkCore;

// using var context = new AppDbContext();
// await context.Database.EnsureCreatedAsync();

// if (!await context.Veiculos.AnyAsync())
// {
//     var seed = new List<Veiculo>
//     {
//         new Veiculo { Id = Guid.NewGuid(), Placa = "ABC-1234", Modelo = "Gol G6", Preco = 25000m },
//         new Veiculo { Id = Guid.NewGuid(), Placa = "XYZ-9876", Modelo = "Civic 2015", Preco = 45000m }
//     };
//     context.Veiculos.AddRange(seed);
//     await context.SaveChangesAsync();
//     Console.WriteLine("Seed inicial aplicada: 2 veículos.");
// }
// var importList = new List<Veiculo>
// {
//     new Veiculo { Id = Guid.NewGuid(), Placa = "ABC-1235", Modelo = "Gol Bolinha", Preco = 27000m },
//     new Veiculo { Id = Guid.NewGuid(), Placa = "NEW-2026", Modelo = "Fiat Pulse", Preco = 82000m }
// };

// Console.WriteLine("Importando lista da locadora (Upsert por Placa)...");

// var config = new BulkConfig
// {
//     BatchSize = 1000,
//     CalculateStats = true,
//     SetOutputIdentity = false,
//     UpdateByProperties = new List<string> { "Placa" }
// };

// await context.BulkInsertOrUpdateAsync(importList, config);

// Console.WriteLine($"Inseridos: {config.StatsInfo?.StatsNumberInserted ?? 0}");
// Console.WriteLine($"Atualizados: {config.StatsInfo?.StatsNumberUpdated ?? 0}");

// var todos = await context.Veiculos.AsNoTracking().ToListAsync();
// Console.WriteLine("Veículos atualmente no banco:");
// foreach (var v in todos)
// {
//     Console.WriteLine($"Placa: {v.Placa} | Modelo: {v.Modelo} | Preço: {v.Preco}");
// }
