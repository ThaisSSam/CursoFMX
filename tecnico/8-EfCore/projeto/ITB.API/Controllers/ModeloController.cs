using ITB.Application.Commands;
using ITB.Application.Dtos;
using ITB.Application.Interfaces;
using ITB.Domain.Core.Messages.Interfaces;
using ITB.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ITB.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ModeloController : ControllerBase
{
    private readonly IModeloRepository _repository;
    private readonly IMessageBus _bus;
    private readonly IModeloQuery _query;

    public ModeloController(IModeloRepository repository, IMessageBus bus, IModeloQuery query)
    {
        _repository = repository;
        _bus = bus;
        _query = query;
    }

    [HttpGet("dropdown")]
    public async Task<IActionResult> ObterModelosComMarca()
    {
        var modelos = await _query.ObterModelosParaDropdown();

        return Ok(modelos);
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var modelos = await _repository.ObterTodosAsync();

        var modelosDto = modelos?
            .Select(m => new ModeloDTO
            {
                Id = m.Id,
                Nome = m.Nome,
                MarcaId = m.MarcaId,
                Ativo = m.Ativo,

                Marca = new MarcaDTO()
                {
                    Id = m.Marca.Id,
                    Nome = m.Marca.Nome,
                    Ativo = m.Marca.Ativo
                }
            });

        return Ok(modelosDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ModeloReadDTO>> GetById(int id)
    {
        var modelo = await _repository.ObterPorIdAsync(id);

        if (modelo == null)
            return NotFound(new { message = "Modelo não encontrado." });

        return Ok(modelo);
    }

    // [HttpPost]
    // public async Task<ActionResult> Post([FromBody] AdicionarModeloCommand comando)
    // {
    //     var resultado = await _bus.EnviarComando(comando);
    //     if (!resultado.Sucesso)
    //         return BadRequest(resultado);

    //     return Ok(new { message = resultado.Mensagem });
    // }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] AtualizarModeloCommand command)
    {
        if (id != command.Id) return BadRequest(new { mensagem = "IDs divergentes." });

        await _bus.EnviarComando(command);
        return Ok(new { mensagem = "Modelo atualizado com sucesso." });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new DesativarModeloCommand { Id = id };

        await _bus.EnviarComando(command);
        return Ok(new { mensagem = "Modelo desativado com sucesso." });
    }

}