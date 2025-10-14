using System;
using LojaApi.Data;
using LojaApi.Entities;
using LojaApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LojaApi.Repositories;

public class ProdutoDBRepository : IProdutoRepository
{
    private readonly LojaContext _context;
    public ProdutoDBRepository(LojaContext context) { _context = context; }

    public List<Produto> ObterTodos()
    {
        // Eager Loading: Traz os dados da Categoria junto com os Produtos. 
        return _context.Produtos.Include(p => p.Categoria).ToList();
    }

    public Produto? ObterPorId(int id)
    {
        return _context.Produtos.Include(p => p.Categoria).FirstOrDefault(p => p.Id == id);
    }

    public Produto Adicionar(Produto novoProduto)
    {
        _context.Produtos.Add(novoProduto);
        _context.SaveChanges();
        return novoProduto;
    }
}
