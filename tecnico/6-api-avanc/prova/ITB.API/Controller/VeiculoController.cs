using System;
using ITB.Application.Commands;
using ITB.Domain.Core.Messages.Interfaces;
using ITB.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITB.API.Controller;

[ApiController]
[Route("api/[controller]")]
public class VeiculoController : ControllerBase
{
    private readonly IMessageBus _bus;

    private readonly AppDbContext _context;

    public VeiculoController(IMessageBus bus, AppDbContext context)
    {
        _bus = bus;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Get() => Ok(await _context.veiculos.ToListAsync());

    [HttpPost]
    public async Task<IActionResult> Post([FromBody]AdicionarVeiculoCommand command)
    {
        await _bus.EnviarComando(command);
        return Ok("Veículo criado");
    }
}
