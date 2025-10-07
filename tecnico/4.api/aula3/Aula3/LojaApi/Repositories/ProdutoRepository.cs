using System;
using LojaApi.Entities;
using LojaApi.Repositories.Interfaces;

public class ProdutoRepository : IProdutoRepository
{
    private readonly
    List<Produto> _produtos = new List<Produto>
    {
        new Produto{
            Id= 1,
            Nome= "Mouse",
            Valor= 40.9m,
            Descricao= "Mouse com fio",
            Estoque= 15,
            Deletado= false 
        }
    };

    private int _nextId = 2;
    public List<Produto> GetAll() => _produtos;

    public Produto? GetById(int id) => _produtos.FirstOrDefault(c => c.Id == id);

    public Produto Add(Produto novoProduto)
    {
        novoProduto.Id = _nextId++;
        _produtos.Add(novoProduto);
        return novoProduto;
    }

    public Produto? Update(int id, Produto produtoAtualizado)
    {
        var produtoExistente = GetById(id);
        if (produtoExistente == null) return null;
        produtoExistente.Nome = produtoAtualizado.Nome;
        produtoExistente.Valor = produtoAtualizado.Valor;
        produtoExistente.Descricao = produtoAtualizado.Descricao;
        produtoExistente.Estoque = produtoAtualizado.Estoque;
        return produtoExistente;
    }

    public bool Delete(int id)
    {
        var produtoParaDeletar = GetById(id);
        if (produtoParaDeletar == null) return false;

        _produtos.Remove(produtoParaDeletar);
        return true;
    }
}
