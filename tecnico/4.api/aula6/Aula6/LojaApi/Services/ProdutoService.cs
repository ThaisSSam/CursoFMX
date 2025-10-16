using System;
using LojaApi.Entities;
using LojaApi.Infra.DTOs;
using LojaApi.Infra.Repositories.Interfaces;
using LojaApi.Services.Interfaces;

namespace LojaApi.Services;

public class ProdutoService : IProdutoService
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly ICategoriaRepository _categoriaRepository; // Dependência para validar a categoria 

    public ProdutoService(
        IProdutoRepository produtoRepository,
        ICategoriaRepository categoriaRepository)
    {
        _produtoRepository = produtoRepository;
        _categoriaRepository = categoriaRepository;
    }

    public List<Produto> ObterTodos()
    {
        return _produtoRepository.ObterTodos().Where(p => p.Estoque > 0).ToList();
    }

    public Produto? ObterPorId(int id)
    {
        return _produtoRepository.ObterPorId(id);
    }

    public Produto Adicionar(CriarProduto produtoDto)
    {
        var novoProduto = new Produto
        {
            Nome = produtoDto.Nome.ToUpper(),
            Preco = produtoDto.Preco,
            Estoque = produtoDto.Estoque,
            CategoriaId = produtoDto.CategoriaId
        };

        var categoria = _categoriaRepository.ObterPorId(novoProduto.CategoriaId);
        if (categoria == null)
        {
            // Em um projeto real, lançaríamos uma exceção customizada que vamos ver mais à frente no curso. 
            // Por simplicidade, podemos retornar null ou uma mensagem. 
            throw new Exception("A categoria informada não existe.");
        }

        if (categoria.Nome.Equals("Eletrônicos", StringComparison.OrdinalIgnoreCase) && novoProduto.Preco < 50.00m)
        {
            throw new Exception("Produtos da categoria 'Eletrônicos' devem custar no mínimo R$ 50,00.");
        }

        return _produtoRepository.Adicionar(novoProduto);
    }
    
    public ProdutoDetalhado? ObterDetalhesPorId(int id)
    {
        var produto = _produtoRepository.ObterPorId(id);
        if (produto == null) return null;

        return new ProdutoDetalhado
        {
            Id = produto.Id,
            Nome = produto.Nome.ToUpper(),
            Preco = produto.Preco,
            Estoque = produto.Estoque,
            CategoriaId = produto.CategoriaId
        };

        var produtoSalvo = _produtoRepository.Adicionar(novoProduto);

        var produtoRetorno = _produtoRepository.ObterPorId(novoProduto.Id);
        var produtoRetornoDto = new ProdutoDTO
        {
            Nome = produtoRetorno.Nome,
            Preco = produtoRetorno.Preco == null ? 0 : produto.Preco
        };

        return produtoRetornoDto;
    }
}
