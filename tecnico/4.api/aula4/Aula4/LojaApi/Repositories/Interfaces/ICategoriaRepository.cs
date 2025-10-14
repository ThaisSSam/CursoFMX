using System;
using LojaApi.Entities;

namespace LojaApi.Repositories.Interfaces;

public interface ICategoriaRepository
{
    Categoria? ObterPorId(int id);
    List<Categoria> ObterTodos();
    Categoria Adicionar(Categoria novaCategoria);
}
