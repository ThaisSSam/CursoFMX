using ITB.Application.Commands;
using ITB.Application.Dtos;
using ITB.Application.Interfaces;
using ITB.Domain.Core.Messages.Interfaces;
using ITB.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ITB.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MarcaController : ControllerBase
{
    private readonly IMarcaRepository _repository;
    private readonly IMessageBus _bus;
    private readonly IMarcaQuery _query;

    public MarcaController(IMarcaRepository repository, IMessageBus bus, IMarcaQuery query)
    {
        _repository = repository;
        _bus = bus;
        _query = query;
    }

    [HttpGet("marcas-veiculos")]
    public async Task<IActionResult> ObterMarcasComVeiculos()
    {
        var veiculos = await _query.ObterMarcasComVeiculosProjecao();

        return Ok(veiculos);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MarcaReadDto>>> Get()
    {
        var marcas = await _repository.ObterTodosAsync();
        var marcasDto = marcas?.Select(m => new MarcaReadDto
        {
            Id = m.Id,
            Nome = m.Nome,
            Ativo = m.Ativo,
            Veiculos = m.Veiculos
        });
        return Ok(marcasDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MarcaReadDto>> GetById(int id)
    {
        var marca = await _repository.ObterPorIdAsync(id);

        if (marca == null)
            return NotFound(new { message = "Marca não encontrada." });

        return Ok(marca);
    }

    // [HttpPost]
    // public async Task<ActionResult> Post([FromBody] AdicionarMarcaCommand comando)
    // {
    //     var resultado = await _bus.EnviarComando(comando);

    //     if (!resultado.Sucesso)
    //         return BadRequest(resultado);

    //     return Ok(new { id = resultado.Dados, message = resultado.Mensagem });
    // }

    [HttpDelete("{id}")]
    public async Task<ActionResult<MarcaReadDto>> DeleteById(int id)
    {
        var marca = await _repository.ObterPorIdAsync(id);

        if (marca == null)
            return NotFound(new { message = "Marca não encontrada." });

        await _bus.EnviarComando(new DeletarMarcaCommand
        {
            Id = id
        });

        return NoContent();
    }

}
