using System;
using ApiEstoque.Entities;

namespace ApiEstoque.Infra.Repositories.Interfaces;

public interface IProdutoDBRepository
{
    Task<List<Produto>> ObterTodosAsync();

    Task<Produto> ObterPorIdAsync(int id);

    Task <Produto> AdicionarAsync(Produto novoProduto);

    Task<Produto> AtualizarAsync(Produto produto);

    Task DeletarAsync(Produto Produto);
    Task ObterFabricantePorIdAsync(int FabricanteId);
}
