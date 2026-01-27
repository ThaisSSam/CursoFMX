using System;
using ApiEstoque.Infra.DTOs;

namespace ApiEstoque.Services.Interfaces;

public interface IEstoqueService
{
    Task<List<ProdutoDto>> ObterTodosAsync();

    Task<ProdutoDto?> ObterPorIdAsync(int id);

    Task<ProdutoDto> AdicionarAsync(CriarProdutoDto produtoDto);

    Task DeletarAsync(int id);
    Task ObterFabricantePorIdAsync(int id);
}
