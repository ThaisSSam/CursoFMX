using System;
using LojaApi.Data;
using LojaApi.Entities;
using LojaApi.Repositories.Interfaces;

namespace LojaApi.Repositories;

public class ProdutoDBRepository : IProdutoRepository
{
    private readonly LojaContext _context;

    public ProdutoDBRepository(LojaContext context)
    {
        _context = context;
    }

    public List<Produto> GetAll() 
    { 
        return _context.Produtos.ToList(); 
    } 

    public Produto? GetById(int id) 
    { 
        return _context.Produtos.FirstOrDefault(c => c.Id == id); 
    } 

    public Produto Add(Produto novoProduto)
    {
        novoProduto.Deletado = false;
        _context.Produtos.Add(novoProduto); 
        _context.SaveChanges();  
        return novoProduto; 
    } 

    public Produto? Update(int id, Produto produtoAtualizado) 
    {
        // O serviço já carregou e alterou a entidade. O repositório apenas persiste. 
        _context.Produtos.Update(produtoAtualizado); 
        _context.SaveChanges(); 
        return produtoAtualizado; 
    } 

    public bool Delete(int id) 
    { 
        var produtoParaDeletar = GetById(id); 
        if (produtoParaDeletar == null) return false; 

        _context.Produtos.Remove(produtoParaDeletar); 
        _context.SaveChanges(); 
        return true; 
    } 
}
