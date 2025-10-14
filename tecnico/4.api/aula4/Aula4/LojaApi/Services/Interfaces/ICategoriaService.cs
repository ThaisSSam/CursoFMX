using System;
using LojaApi.Entities;

namespace LojaApi.Services.Interfaces;

public interface ICategoriaService
{
    Categoria? ObterPorId(int id);
    List<Categoria> ObterTodos();
    Categoria Adicionar(Categoria novaCategoria);
}
