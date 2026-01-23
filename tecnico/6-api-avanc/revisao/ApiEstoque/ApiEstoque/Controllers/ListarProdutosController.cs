using ApiEstoque.Entities;
using ApiEstoque.Infra.Context;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/ListarProdutos")] // ERRO: Verbo na URI
public class ProdutoController : ControllerBase {
    private readonly LojaDbContext _db;

    public ProdutoController(LojaDbContext db) { _db = db; } // ERRO: Injeção de Contexto (DIP violado)

    [HttpGet]
    public List<Produto> Get() {
        // ERRO: Retornando Entidade diretamente (Risco de Ciclo e Segurança)
        // ERRO: Problema N+1 (Falta o .Include)
        // ERRO: Falta o Service
        return _db.Produtos.ToList(); 
    }
}