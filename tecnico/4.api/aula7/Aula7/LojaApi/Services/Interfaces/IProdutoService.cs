using System;
using LojaApi.Entities;
using LojaApi.Infra.DTOs;

namespace LojaApi.Services.Interfaces;

public interface IProdutoService
{
    List<ProdutoResumoDto> ObterTodos();
    Produto? ObterPorId(int id);
    Produto Adicionar(Produto novoProduto);
}
