using LojaApi.Entities;
using LojaApi.Infra.DTOs;
using LojaApi.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LojaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutoService _produtoService;
        public ProdutosController(IProdutoService produtoService)
        {
            _produtoService = produtoService;
        }

        [HttpGet]
        public ActionResult<List<ProdutoResumoDto>> GetAll()
        {
            return Ok(_produtoService.ObterTodos());
        }

        [HttpGet("{id}")]
        public ActionResult<Produto> GetById(int id)
        {
            var produto = _produtoService.ObterPorId(id);
            if (produto == null) return NotFound();
            return Ok(produto);
        }

        [HttpPost]
        public ActionResult<Produto> Add(Produto novoProduto)
        {
            // A validação de formato é automática! 
            // Agora, tratamos os erros de negócio que podem vir do serviço. 
            try
            {
                var produtoAdicionado = _produtoService.Adicionar(novoProduto);
                return CreatedAtAction(nameof(GetById), new
                {
                    id = produtoAdicionado.Id
                }, produtoAdicionado);
            }
            catch (Exception ex)
            {
                // Retorna 400 Bad Request com a mensagem de erro de negócio. 
                return BadRequest(ex.Message);
            }
        }
    }
}
