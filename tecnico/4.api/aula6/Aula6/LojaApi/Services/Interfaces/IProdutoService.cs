using System;
using LojaApi.Entities;
using LojaApi.Infra.DTOs;

namespace LojaApi.Services.Interfaces;

public interface IProdutoService
{
    List<Produto> ObterTodos();
    Produto? ObterPorId(int id);
    Produto Adicionar(CriarProduto produtoDto);

    ProdutoDetalhado? ObterDetalhesPorId(int id);
}
