using ApiEstoque.Entities;
using ApiEstoque.Infra.Context;
using ApiEstoque.Infra.DTOs;
using ApiEstoque.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ProdutosController : ControllerBase 
{    
    private readonly IEstoqueService _estoqueService;

    public ProdutosController(IEstoqueService estoqueService)
    {
        _estoqueService = estoqueService;
    }

    [HttpGet]
    public ActionResult<List<ProdutoDto>> ObterTodos()
    {
    
        return Ok(_estoqueService.ObterTodos());
    }

    [HttpGet("{id}")]
    public ActionResult<ProdutoDto> ObterPorId(int id)
    {
        var produto = _estoqueService.ObterPorId(id);

        if(produto == null)
        {
            return NotFound();
        }
        return Ok(produto);
    }

    [HttpPost]
    public ActionResult<ProdutoDto>Adicionar([FromBody] CriarProdutoDto produtoDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var produtoAdicionado = _estoqueService.Adicionar(produtoDto);

        return CreatedAtAction(nameof(ObterPorId), new{id = produtoAdicionado.Id}, produtoAdicionado);
    } 

    [HttpDelete("{id}")]
    public ActionResult Deletar(int id)
    {
        var deletado = _estoqueService.Deletar(id);

        if(!deletado)
        {
            return NotFound();
        }

        return Ok(new{mensagem = "Deletado com sucesso"});
    }
}