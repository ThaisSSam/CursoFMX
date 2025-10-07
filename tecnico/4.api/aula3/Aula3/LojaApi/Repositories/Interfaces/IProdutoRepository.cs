using System;
using LojaApi.Entities;

namespace LojaApi.Repositories.Interfaces;

public interface IProdutoRepository
{
    List<Produto> GetAll();
    Produto? GetById(int id);
    Produto Add(Produto novoProduto);
    Produto? Update(int id, Produto produtoAtualizado);
    bool Delete(int id);
}
