using System;
using LojaApi.Entities;
using LojaApi.Infra.Repositories.Interfaces;
using LojaApi.Services.Interfaces;

namespace LojaApi.Services;

public class CategoriaService : ICategoriaService
{
    private readonly ICategoriaRepository _categoriaRepository;

    public CategoriaService(ICategoriaRepository categoriaRepository)
    {
        _categoriaRepository = categoriaRepository;
    }

    public Categoria Adicionar(Categoria novaCategoria)
    {
        return _categoriaRepository.Adicionar(novaCategoria);
    }

    public Categoria? ObterPorId(int id)
    {
        return _categoriaRepository.ObterPorId(id);
    }

    public List<Categoria> ObterTodos()
    {
        return _categoriaRepository.ObterTodos();
    }
}
