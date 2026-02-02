using System.Threading.Tasks;
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
    public async Task<ActionResult<List<ProdutoDto>>> ObterTodosAsync()
    {
    
        return Ok(await _estoqueService.ObterTodosAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProdutoDto>> ObterPorIdAsync(int id)
    {
        var produto = await _estoqueService.ObterPorIdAsync(id);

        if(produto == null)
        {
            return NotFound();
        }
        return Ok(produto);
    }

    [HttpPost]
    public async Task<ActionResult<ProdutoDto>> AdicionarAsync([FromBody] CriarProdutoDto produtoDto)
    {
        var produtoAdicionado = await _estoqueService.AdicionarAsync(produtoDto);

        return CreatedAtAction("ObterPorId", new { id = produtoAdicionado.Id }, produtoAdicionado);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Deletar(int id)
    {
        await _estoqueService.DeletarAsync(id);

        return Ok(new{mensagem = "Deletado com sucesso"});
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ProdutoDto>> AtualizarAsync(int id, [FromBody] CriarProdutoDto produtoDto)
    {
        var resultado = await _estoqueService.AtualizarAsync(id, produtoDto);

        if (resultado == null)
        {
            return NotFound(new { mensagem = "Produto não encontrado." });
        }

        return Ok(resultado);
    }
}