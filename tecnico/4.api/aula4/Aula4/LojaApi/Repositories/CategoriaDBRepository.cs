using LojaApi.Data;
using LojaApi.Entities;
using LojaApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

public class CategoriaDBRepository : ICategoriaRepository
{
    public readonly LojaContext _context;

    public CategoriaDBRepository(LojaContext context)
    {
        _context = context;
    }

    public Categoria? ObterPorId(int id)
    {
        return _context.Categorias.FirstOrDefault(c => c.Id == id);
    }

    public List<Categoria> ObterTodos()
    {
        return _context.Categorias.Include(p => p.Produtos).ToList();
    }

    public Categoria Adicionar(Categoria novaCategoria)
    {
        _context.Categorias.Add(novaCategoria);
        _context.SaveChanges();
        return novaCategoria;
    }

    public Categoria? Atualizar(int id, Categoria categoriaAtualizada)
    {
        _context.Categorias.Update(categoriaAtualizada);
        _context.SaveChanges();
        return categoriaAtualizada;
    }

}