using System;
using ITB.Application.Commands;
using ITB.Application.Dtos;
using ITB.Domain.Core.Messages.Interfaces;
using ITB.Domain.Entities;
using ITB.Domain.Interfaces;
using ITB.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ITB.API.Controller;

[ApiController]
[Route("api/[controller]")]
public class ModeloController :ControllerBase
{
    private readonly IMessageBus _bus;

    private readonly AppDbContext _context;

    private readonly IModeloRepository _modeloRepository;

    public ModeloController(IMessageBus bus, AppDbContext context, IModeloRepository modeloRepository)
    {
        _bus = bus;
        _context = context;
        _modeloRepository = modeloRepository;
    }

    [HttpGet]
    public async Task <IActionResult> Get()
    {
        var modelos = await _modeloRepository.ObterTodosAsync();

        var modelosDTO = modelos.Select(m => new ModeloDTO{
            Id = m.Id,
            Nome = m.Nome,
            Ativo=m.Ativo,

            Marca = m.Marca != null ? new MarcaDTO 
            {
                Id = m.Marca.Id,
                Nome = m.Marca.Nome
            } : null
        });
        return Ok (modelosDTO);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] AdicionarModeloCommand command)
    {
        await _bus.EnviarComando(command);
        return Ok("Modelo criado");
    }
}
