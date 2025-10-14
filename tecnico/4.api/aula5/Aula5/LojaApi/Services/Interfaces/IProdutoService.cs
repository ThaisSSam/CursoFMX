using System;
using LojaApi.Entities;

namespace LojaApi.Services.Interfaces;

public interface IProdutoService
{
    List<Produto> ObterTodos();
    Produto? ObterPorId(int id);
    Produto Adicionar(Produto novoProduto);
}
