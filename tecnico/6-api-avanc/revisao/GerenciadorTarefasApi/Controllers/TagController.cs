using GerenciadorTarefasApi.Infra.DTOs;
using GerenciadorTarefasApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace GerenciadorTarefasApi.Controllers
{
    [ApiController]
    [Route("api/tags")]
    public class TagsController : ControllerBase
    {
        private readonly ITagService _tagService;

        public TagsController(ITagService tagService)
        {
            _tagService = tagService;
        }

        // GET /api/tags
        [HttpGet]
        public ActionResult<List<TagDto>> ObterTodos()
        {
            var tags = _tagService.ObterTodos();
            return Ok(tags);
        }

        // GET /api/tags/{id}
        [HttpGet("{id}")]
        public ActionResult<TagDto> ObterPorId(int id)
        {
            var tag = _tagService.ObterPorId(id);

            if (tag == null)
            {
                return NotFound();
            }

            return Ok(tag);
        }

        // POST /api/tags
        [HttpPost]
        public ActionResult<TagDto> Adicionar([FromBody] CriarTagDto tagDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tagAdicionada = _tagService.Adicionar(tagDto);

            return CreatedAtAction(nameof(ObterPorId), new { id = tagAdicionada.Id }, tagAdicionada);
        }

        // PUT /api/tags/{id}
        [HttpPut("{id}")]
        public ActionResult<TagDto> Atualizar(int id, [FromBody] CriarTagDto tagDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tagAtualizada = _tagService.Atualizar(id, tagDto);

            if (tagAtualizada == null)
            {
                return NotFound();
            }

            return Ok(tagAtualizada);
        }

        // DELETE /api/tags/{id}
        [HttpDelete("{id}")]
        public ActionResult Deletar(int id)
        {
            var sucesso = _tagService.Deletar(id);

            if (!sucesso)
            {
                return NotFound();
            }

            return Ok(new { mensagem = "Deletado com sucesso!" });
        }
    }
}