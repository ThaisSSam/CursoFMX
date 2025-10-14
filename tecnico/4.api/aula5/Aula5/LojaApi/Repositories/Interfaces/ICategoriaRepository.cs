using System;
using LojaApi.Entities;

namespace LojaApi.Repositories.Interfaces;

public interface ICategoriaRepository
{
    List<Categoria> ObterTodos();
    Categoria? ObterPorId(int id);
    Categoria Adicionar(Categoria novaCategoria);
}
