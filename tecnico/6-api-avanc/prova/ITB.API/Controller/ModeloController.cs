using System;
using ITB.Application.Commands;
using ITB.Application.Dtos;
using ITB.Application.Interfaces;
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
    private readonly IModeloQuery _query;

    private readonly AppDbContext _context;

    private readonly IModeloRepository _modeloRepository;

    public ModeloController(IMessageBus bus, AppDbContext context, IModeloRepository modeloRepository, IModeloQuery query)
    {
        _bus = bus;
        _context = context;
        _modeloRepository = modeloRepository;
        _query = query;
    }

    [HttpGet]
    // public async Task <IActionResult> Get()
    // {
    //     var modelos = await _modeloRepository.ObterTodosAsync();

    //     var modelosDTO = modelos.Select(m => new ModeloDTO{
    //         Id = m.Id,
    //         Nome = m.Nome,
    //         Ativo=m.Ativo,

    //         Marca = m.Marca != null ? new MarcaDTO 
    //         {
    //             Id = m.Marca.Id,
    //             Nome = m.Marca.Nome
    //         } : null
    //     });
    //     return Ok (modelosDTO);
    // }

    // Novo com o Query
    public async Task<IActionResult> ObterTodosAsync()
    {
        var modelos = await _query.ObterTodosAsync();
        return Ok(modelos);
    }

    [HttpPost]
    // public async Task<IActionResult> Post([FromBody] AdicionarModeloCommand command)
    // {
    //     await _bus.EnviarComando(command);
    //     return Ok("Modelo criado");
    // }
    // Novo com o Query
    public async Task<IActionResult> Adicionar([FromBody] AdicionarModeloCommand command)
    {
        await _bus.EnviarComando(command);
        return Ok(new { mensagem = "Modelo enviado para processamento." });
    }
}
