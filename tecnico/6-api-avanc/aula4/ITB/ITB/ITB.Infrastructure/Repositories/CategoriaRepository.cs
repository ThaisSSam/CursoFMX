using System;
using ITB.Domain.Entities;
using ITB.Domain.Interfaces;
using ITB.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ITB.Infrastructure.Repositories;

public class CategoriaRepository : ICategoriaRepository
{
    private readonly AppDbContext _context;

    public CategoriaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AdicionarAsync(Categoria novaCategoria)
    {
        await _context.Categorias.AddAsync(novaCategoria);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(Categoria categoriaAtualizada){
        _context.Categorias.Update(categoriaAtualizada);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Categoria>> ObterTodosAsync()
    {
        return await _context.Categorias.ToListAsync();
    }

    public async Task<Categoria?>ObterPorIdAsync(int id)
    {
        return await _context.Categorias.FindAsync(id);
    }

    public async Task<bool> RemoverAsync(int id)
    {
        // Executa o DELETE direto no PostgreSQL em uma única linha
        // Retorna a quantidade de linhas afetadas (0 se não existia, 1 se deletou)
        var linhasAfetadas = await _context.Categorias
            .Where(f => f.Id == id)
            .ExecuteDeleteAsync();

        return linhasAfetadas > 0;
    }
}
