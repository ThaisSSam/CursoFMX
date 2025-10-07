using System;
using System.Globalization;
using LojaApi.Entities;
using LojaApi.Repositories.Interfaces;
namespace LojaApi.Services.Interfaces;

public class ProdutoService : IProdutoService
{
    private readonly IProdutoRepository _produtoRepository;

    public ProdutoService(IProdutoRepository produtoRepository)
    {
        _produtoRepository = produtoRepository;
    }

    public List<Produto> GetAll()
    {
        return _produtoRepository.GetAll().Where(c =>c.Deletado == false).ToList();
    }

    public Produto? GetById(int id)
    {
        return _produtoRepository.GetById(id);
    }

    public Produto Add(Produto novoProduto)
    {
        novoProduto.Nome = novoProduto.Nome.ToUpper();
        novoProduto.Descricao = novoProduto.Descricao.ToUpper();
        novoProduto.Deletado = false;
        return _produtoRepository.Add(novoProduto);
    }

    public Produto? Update(int id, Produto produtoAtualizado)
    {
        if (id != produtoAtualizado.Id) return null;
        return _produtoRepository.Update(id, produtoAtualizado);
    }

    public bool Delete(int id)
    {
        var produto = _produtoRepository.GetById(id);
        if (produto != null)
        {
            produto.Deletado = true;
            return _produtoRepository.Update(id, produto) != null;
        }
        return false;
    }
}
