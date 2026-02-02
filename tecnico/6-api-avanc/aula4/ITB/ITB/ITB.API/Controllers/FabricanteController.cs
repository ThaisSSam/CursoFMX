using ITB.Application.Dtos;
using ITB.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ITB.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FabricanteController : ControllerBase
{
    private readonly IFabricanteService _service;

    public FabricanteController(IFabricanteService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<FabricanteReadDto>>> Get()
    {
        var fabricantes = await _service.ObterTodosAsync();
        return Ok(fabricantes);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<FabricanteReadDto>> GetById(int id)
    {
        var fabricante = await _service.ObterPorIdAsync(id);
        
        if (fabricante == null)
            return NotFound(new { message = "Fabricante não encontrado." });

        return Ok(fabricante);
    }

    [HttpPost]
    public async Task<ActionResult<FabricanteReadDto>> Post([FromBody] FabricanteCreateDto dto)
    {
        var novoFabricante = await _service.AdicionarAsync(dto);
        
        // Retorna o status 201 (Created) e o link para o GetById
        return CreatedAtAction(nameof(GetById), new { id = novoFabricante.Id }, novoFabricante);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] FabricanteUpdateDto dto)
    {
        if (id != dto.Id)
            return BadRequest(new { message = "O ID do corpo da requisição deve ser igual ao ID da URL." });

        var atualizado = await _service.AtualizarAsync(dto);

        if (!atualizado)
            return NotFound(new { message = "Fabricante não encontrado para atualização." });

        return NoContent(); // 204: Sucesso, mas sem conteúdo no retorno
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var removido = await _service.RemoverAsync(id);

        if (!removido)
            return NotFound(new { message = "Fabricante não encontrado para exclusão." });

        return NoContent();
    }
}