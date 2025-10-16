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
        public ActionResult<List<Produto>> GetAll()
        {
            return Ok(_produtoService.ObterTodos());
        }

        [HttpGet("{id}")]
        public ActionResult<ProdutoDetalhado> GetById(int id)
        {
            var produtoDto = _produtoService.ObterDetalhesPorId(id);
            if (produtoDto == null) return NotFound();
            return Ok(produtoDto);
        }

        [HttpPost]
        public ActionResult<ProdutoDetalhado> Add(CriarProduto produtoDto)
        {
                var produtoCriado = _produtoService.Adicionar(produtoDto);
            var dtoRetorno = _produtoService.ObterDetalhesPorId(produtoCriado.Id);
            return CreatedAtAction(nameof(GetById), new { id = produtoCriado.Id }, dtoRetorno);
        }
    }
}
