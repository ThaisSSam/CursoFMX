using System;
using ApiEstoque.Entities;

namespace ApiEstoque.Infra.Repositories.Interfaces;

public interface IProdutoDBRepository
{
    List<Produto> ObterTodos();

    Produto ObterPorId(int id);

    Produto Adicionar(Produto novoProduto);

    void Deletar(Produto Produto);
}
