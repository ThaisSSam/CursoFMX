using Microsoft.AspNetCore.Mvc;
using ITB.Domain.Core.Messages.Interfaces;
using ITB.Application.Commands;
using ITB.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ITB.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FabricanteController : ControllerBase
{
    private readonly IMessageBus _bus;
    private readonly AppDbContext _context;

    public FabricanteController(IMessageBus bus, AppDbContext context)
    {
        _bus = bus;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Get() => Ok(await _context.fabricantes.ToListAsync());

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CriarFabricanteCommand command)
    {
        await _bus.EnviarComando(command);
        return Ok("Fabricante criado com sucesso!");
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] AtualizarFabricanteCommand command)
    {
        command.Id = id;

        await _bus.EnviarComando(command);
        return Ok();
    }   

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _bus.EnviarComando(new DeletarFabricanteCommand(id));
        return Ok("Fabricante removido!");
    }
}
