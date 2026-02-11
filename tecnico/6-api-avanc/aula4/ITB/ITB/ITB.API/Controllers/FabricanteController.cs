using ITB.Application.Commands;
using ITB.Application.Dtos;
using ITB.Application.Interfaces;
using ITB.Domain.Core.Messages;
using Microsoft.AspNetCore.Mvc;
using ZstdSharp.Unsafe;

namespace ITB.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FabricanteController : ControllerBase
{
    private readonly IFabricanteService _service;
    private readonly IMessageBus _bus;

    public FabricanteController(IFabricanteService service, IMessageBus bus)
    {
        _service = service;
        _bus = bus;
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
    public async Task<IActionResult> Post([FromBody] CriarFabricanteCommand command)
    {
        await _bus.EnviarComando(command);
        return Ok();
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