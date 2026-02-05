using System;
using ITB.Application.Dtos;
using ITB.Application.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace ITB.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProdutosController : ControllerBase {
    private readonly IProdutoService _service;

    public ProdutosController(IProdutoService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProdutoReadDto>>> Get() => 
    
        Ok(await _service.ObterTodosAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult<ProdutoReadDto>> GetById(int id) {
        var p = await _service.ObterPorIdAsync(id);
        return p != null ? Ok(p) : NotFound();
    }

    [HttpPost]
    public async Task<ActionResult<ProdutoReadDto>> Post(ProdutoCreateDto dto) {
        var p = await _service.AdicionarAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = p.id }, p);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, ProdutoUpdateDto dto) => 
        await _service.AtualizarAsync(id, dto) ? NoContent() : NotFound();

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) => 
        await _service.DeletarAsync(id) ? NoContent() : NotFound();
}