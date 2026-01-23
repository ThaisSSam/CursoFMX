using System;
using ApiEstoque.Entities;
using ApiEstoque.Infra.Context;
using ApiEstoque.Infra.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApiEstoque.Infra.Repositories;

public class ProdutoDBRepository : IProdutoDBRepository
{
    private readonly LojaDbContext _context;

    public ProdutoDBRepository(LojaDbContext context)
    {
        _context = context;
    }

    public List<Produto> ObterTodos()
    {
        return _context.Produtos.Include(p => p.Fabricante).ToList();
    }

    public Produto? ObterPorId(int id)
    {
        var produto = _context.Produtos.Include(p => p.Fabricante).ToList();
        return produto.FirstOrDefault(p => p.Id == id);
    }

    public Produto Adicionar(Produto novoProduto)
    {
        
        var produto = _context.Produtos.Include(p => p.Fabricante).ToList();
        produto.Add(novoProduto);
        _context.SaveChanges();
        return novoProduto;
    }

    public void Deletar(Produto produto)
    {
        _context.Produtos.Remove(produto);
        _context.SaveChanges();
    }


}
