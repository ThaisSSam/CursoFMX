using LojaApi.Entities;
using LojaApi.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LojaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutoService _produtoService;

        public ProdutosController(IProdutoService produtoService)
        {
            _produtoService = produtoService;
        }

        [HttpGet]
        public ActionResult Get()
        {
            return Ok(_produtoService.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<Produto> GetById(int id)
        {
            var produto = _produtoService.GetById(id);
            if (produto == null) return NotFound();
            return Ok(produto);
        }

        [HttpPost]
        public ActionResult<Produto> Add(Produto novoProduto)
        {
            if (string.IsNullOrWhiteSpace(novoProduto.Nome))
            {
                return BadRequest("O nome é obrigatório");
            }
            var produtoCriado = _produtoService.Add( novoProduto);
            return CreatedAtAction(nameof(GetById), new
            { id = produtoCriado.Id }, produtoCriado);
        }

        [HttpPut("{id}")]
        public ActionResult<Produto> Update(int id, Produto produtoAtualizado)
        {
            if (string.IsNullOrWhiteSpace(produtoAtualizado.Nome))
            {
                return BadRequest("O nome é obrigatório");
            }
            var produto = _produtoService.Add(produtoAtualizado);
            if (produto == null) return NotFound();
            return Ok(produto);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var sucesso = _produtoService.Delete(id);
            if (!sucesso) return NotFound();
            return NoContent();
        }
    }
}
