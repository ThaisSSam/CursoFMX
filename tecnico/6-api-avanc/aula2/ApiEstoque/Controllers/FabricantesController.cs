using System;
using ApiEstoque.Entities;
using ApiEstoque.Infra.DTOs;
using ApiEstoque.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ApiEstoque.Controllers;

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

        return CreatedAtAction(nameof(ObterFabricantePorIdAsync), new{id = fabricanteAdicionado.Id}, fabricanteAdicionado);
    } 

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletarFabricanteAsync(int id)
    {
        await _fabricanteService.DeletarFabricanteAsync(id);

        return Ok(new{mensagem = "Deletado com sucesso"});
    }
}
