using System;
using System.Runtime.CompilerServices;
using ITB.Application.Commands;
using ITB.Application.Dtos;
using ITB.Application.Interfaces;
using ITB.Domain.Core.Messages.Interfaces;
using ITB.Infrastructure.Persistence;
using ITB.Infrastructure.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITB.API.Controller;

[ApiController]
[Route("api/[controller]")]
public class MarcaController :ControllerBase
{
    private readonly IMarcaQuery _query;
    private readonly IMessageBus _bus;

    private readonly AppDbContext _context;

    public MarcaController(IMessageBus bus, AppDbContext context, IMarcaQuery query)
    {
        _bus = bus; 
        _context = context;
        _query = query;
    }

    [HttpGet]
    public async Task<IActionResult> Get() =>Ok(await _context.marcas.ToListAsync());    

    [HttpGet ("marca-veiculos")]
    // public async Task<IActionResult> Get() =>Ok(await _context.marcas.ToListAsync());
    public async Task<ActionResult<IEnumerable<MarcaComVeiculosDTO>>> GetMarcaComVeiculosAsync()
    {
        var marcas  = await _query.ObterMarcasComVeiculosAsync();
        return Ok(marcas);
    } 

    [HttpPost]
    public async Task<IActionResult>Post([FromBody] AdicionarMarcaCommand command)
    {
        await _bus.EnviarComando(command);
        return Ok("Marca criada");
    }
}
