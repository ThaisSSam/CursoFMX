using System;
using ApiEstoque.Entities;
using ApiEstoque.Infra.DTOs;
using ApiEstoque.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("api/[controller]")] 

public class FabricantesController : ControllerBase
{
    private readonly IFabricanteService _fabricanteService;

    public FabricantesController(IFabricanteService fabricanteService)
    {
        _fabricanteService = fabricanteService;
    }

    [HttpGet]
    public async Task<ActionResult<List<FabricanteDto>>> ObterTodosFabricanteAsync()
    {
    
        return Ok(await _fabricanteService.ObterTodosFabricanteAsync());
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<FabricanteDto>> ObterFabricantePorIdAsync(int id)
    {
        var fabricante = await _fabricanteService.ObterFabricantePorIdAsync(id);

        if(fabricante == null)
        {
            return NotFound();
        }
        return Ok(fabricante);
    }

    [HttpPost]
    public async Task<ActionResult<FabricanteDto>>AdicionarFabricanteAsync([FromBody] CriarFabricanteDto fabricanteDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var fabricanteAdicionado = await _fabricanteService.AdicionarFabricanteAsync(fabricanteDto);

        return CreatedAtAction("ObterFabricantePorId", new{id = fabricanteAdicionado.Id}, fabricanteAdicionado);
    } 

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletarFabricanteAsync(int id)
    {
        var resultado = await _fabricanteService.DeletarFabricanteAsync(id);

        if (resultado == null)
        {
            return NotFound(new { mensagem = "Fabricante não encontrado para exclusão." });
        }

        return Ok(new { mensagem = "Deletado com sucesso", dados = resultado });
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<FabricanteDto>> AtualizarFabricanteAsync(int id, [FromBody] CriarFabricanteDto fabricanteDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var resultado = await _fabricanteService.AtualizarFabricanteAsync(id, fabricanteDto);

        if (resultado == null)
        {
            return NotFound(new { mensagem = "Fabricante não encontrado para atualização." });
        }

        return Ok(resultado);
    }
}
