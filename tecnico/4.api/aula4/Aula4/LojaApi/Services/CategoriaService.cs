using LojaApi.Entities;
using LojaApi.Repositories.Interfaces;

public class CategoriaService
{
    public readonly ICategoriaRepository _context;

    public CategoriaService(ICategoriaRepository context)
    {
        _context = context;
    }

    public List<Categoria> ObterTodos()
    {
        return _context.ObterTodos();
    }

    public Categoria? ObterPorId(int id)
    {
        return _context.ObterPorId(id);
    }

    public Categoria Adicionar(Categoria novaCategoria)
    {
        // Validação de Negócio: Regras que precisam de acesso a dados. 
        var categoria = _context.ObterPorId(novaCategoria.Id);
        if (categoria == null)
        {

            // Em um projeto real, lançaríamos uma exceção customizada que vamos ver mais à frente no curso. 
            // Por simplicidade, podemos retornar null ou uma mensagem. 
            throw new Exception("A categoria informada não existe.");
        }

        return _context.Adicionar(novaCategoria);
    }

}