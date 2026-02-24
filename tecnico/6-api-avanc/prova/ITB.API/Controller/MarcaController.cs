using System;
using System.Runtime.CompilerServices;
using ITB.Application.Commands;
using ITB.Domain.Core.Messages.Interfaces;
using ITB.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITB.API.Controller;

[ApiController]
[Route("api/[controller]")]
public class MarcaController :ControllerBase
{
    private readonly IMessageBus _bus;

    private readonly AppDbContext _context;

    public MarcaController(IMessageBus bus, AppDbContext context)
    {
        _bus = bus; 
        _context = context; 
    }

    [HttpGet]
    public async Task<IActionResult> Get() =>Ok(await _context.marcas.ToListAsync());

    [HttpPost]
    public async Task<IActionResult>Post([FromBody] AdicionarMarcaCommand command)
    {
        await _bus.EnviarComando(command);
        return Ok("Marca criada");
    }
}
