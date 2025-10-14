using System;
using LojaApi.Data;
using LojaApi.Entities;
using LojaApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LojaApi.Repositories;

public class CategoriaDBRepository : ICategoriaRepository
{
    private readonly LojaContext _context;
    public CategoriaDBRepository(LojaContext context) { _context = context; }

    public List<Categoria> ObterTodos()
    {
        // Eager Loading: Traz os dados da Categoria junto com os Produtos. 
        return _context.Categorias.Include(c => c.Produtos).ToList();
    }

    public Categoria? ObterPorId(int id)
    {
        return _context.Categorias.Include(c => c.Produtos).FirstOrDefault(c => c.Id == id);
    }

    public Categoria Adicionar(Categoria novaCategoria)
    {
        _context.Categorias.Add(novaCategoria);
        _context.SaveChanges();
        return novaCategoria;
    }
}
