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
        // O filtro .FirstOrDefault acontece no Banco de Dados (SQL WHERE)
        return _context.Produtos
            .Include(p => p.Fabricante)
            .FirstOrDefault(p => p.Id == id);
    }


    public Produto Adicionar(Produto novoProduto)
    {
        // 1. Adiciona diretamente ao DbSet do contexto
        _context.Produtos.Add(novoProduto);
        _context.SaveChanges();

        // 2. Carrega explicitamente a referência do fabricante para que ela não seja null no retorno
        _context.Entry(novoProduto).Reference(p => p.Fabricante).Load();

        return novoProduto;
    }

    public void Deletar(Produto produto)
    {
        _context.Produtos.Remove(produto);
        _context.SaveChanges();
    }


}
