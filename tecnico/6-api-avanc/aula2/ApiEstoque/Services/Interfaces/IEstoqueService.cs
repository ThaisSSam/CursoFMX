using System;
using ApiEstoque.Infra.DTOs;

namespace ApiEstoque.Services.Interfaces;

public interface IEstoqueService
{
    List<ProdutoDto> ObterTodos();

    ProdutoDto? ObterPorId(int id);

    ProdutoDto Adicionar(CriarProdutoDto produtoDto);

    bool Deletar(int id);
}
