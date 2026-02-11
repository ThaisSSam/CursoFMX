using ITB.Domain.Entities;
using ITB.Domain.Interfaces;
using ITB.Infrastructure.Persistence; 
using Microsoft.EntityFrameworkCore;

namespace ITB.Infrastructure.Persistence.Repositories;

public class ProdutoRepository : IProdutoRepository
{
    private readonly AppDbContext _context; // Aqui definimos o _context para resolver o erro CS0103

    public ProdutoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Produto>> ObterTodosAsync()
    {
        return await _context.Produtos
        .Include(p => p.fabricante) 
        .ToListAsync();
    }

    public async Task<Produto?> ObterPorIdAsync(int id)
    {
        return await _context.Produtos
        .Include(p => p.fabricante)
        .FirstOrDefaultAsync(p => p.id == id);
    }

    public async Task<Produto> AdicionarAsync(Produto produto)
    {
        await _context.Produtos.AddAsync(produto);
        await _context.SaveChangesAsync();

        await _context.Entry(produto).Reference(p => p.fabricante).LoadAsync();

        return produto;
    }

    public async Task<bool> AtualizarAsync(Produto produto)
    {
        _context.Produtos.Update(produto);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeletarAsync(Produto produto)
    {
        _context.Produtos.Remove(produto);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> QualquerProdutoComFabricante(int fabricanteId)
    {
        return await _context.Produtos.AnyAsync(p => p.fabricanteId == fabricanteId);
    }
}