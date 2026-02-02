using System;
using System.Runtime.InteropServices;
using ITB.Application.Dtos;
using ITB.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ITB.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriaController : ControllerBase
{
    private readonly ICategoriaService _service;

    public CategoriaController(ICategoriaService service){
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoriaReadDto>>> Get()
    {
        var categorias = await _service.ObterTodosAsync();
        return Ok(categorias);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoriaReadDto>> GetById(int id)
    {
        var categoria = await _service.ObterPorIdAsync(id);
        
        if (categoria == null)
            return NotFound(new { message = "Categoria não encontrada." });

        return Ok(categoria);
    }

    [HttpPost]
    public async Task<ActionResult<CategoriaReadDto>> Post([FromBody] CategoriaCreateDto dto)
    {
        var novaCategoria = await _service.AdicionarAsync(dto);
        
        // Retorna o status 201 (Created) e o link para o GetById
        return CreatedAtAction(nameof(GetById), new { id = novaCategoria.Id }, novaCategoria);
    }

    [HttpPut("{id}")]
    public async 
    Task<IActionResult> Put(int id, [FromBody] CategoriaUpdateDto dto)
    {
        if (id != dto.Id)
            return BadRequest(new { message = "O ID do corpo da requisição deve ser igual ao ID da URL." });

        var atualizado = await _service.AtualizarAsync(dto);

        if (!atualizado)
            return NotFound(new { message = "Categoria não encontrada para atualização." });

        return NoContent(); // 204: Sucesso, mas sem conteúdo no retorno
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var removido = await _service.RemoverAsync(id);

        if (!removido)
            return NotFound(new { message = "Categoria não encontrada para exclusão." });

        return NoContent();
    }
}
