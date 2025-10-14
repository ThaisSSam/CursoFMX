using LojaApi.Entities;
using LojaApi.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LojaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaService _categoriaService;
        public CategoriasController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        [HttpGet]
        public ActionResult<List<Categoria>> GetAll()
        {
            return Ok(_categoriaService.ObterTodos());
        }

        [HttpGet("{id}")]
        public ActionResult<Categoria> GetById(int id)
        {
            var categoria = _categoriaService.ObterPorId(id);
            if (categoria == null) return NotFound();
            return Ok(categoria);
        }

        [HttpPost]
        public ActionResult<Categoria> Add(Categoria novaCategoria)
        {
            //  A validação de formato é automática!
            // Agora, tratamos os erros de negócio que podem vir do serviço.
            try
            {
                var categoriaAdicionada = _categoriaService.Adicionar(novaCategoria);
                return CreatedAtAction(nameof(GetById), new
                {
                    id = categoriaAdicionada.Id
                }, categoriaAdicionada);
            }
            catch (Exception ex)
            {
                // Retorna 400 Bad Request com a mensagem de erro de negócio.
                return BadRequest(ex.Message);
            }
        }
    }
}
