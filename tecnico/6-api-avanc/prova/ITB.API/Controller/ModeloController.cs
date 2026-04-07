using System;
using ITB.API.Controller.Base;
using ITB.Application.Commands;
using ITB.Application.Dtos;
using ITB.Application.Handlers;
using ITB.Application.Interfaces;
using ITB.Domain.Core.Messages.Interfaces;
using ITB.Domain.Core.Notifications;
using ITB.Domain.Entities;
using ITB.Domain.Interfaces;
using ITB.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ITB.API.Controller;
// 7. Por fim, o controller que vai receber as requisições e usar as queries para buscar os dados e retornar para o cliente.
[ApiController]
[Route("api/[controller]")]
public class ModeloController : BaseController
{
    private readonly IMessageBus _bus;    
    private readonly AppDbContext _context;

    private readonly IModeloRepository _modeloRepository;

    private readonly IModeloQuery _query;
    private readonly AdicionarModeloHandler _handler;

    private readonly IDomainNotificationHandler<DomainNotification> _notifications;

    public ModeloController(
        IModeloQuery query, 
        AdicionarModeloHandler handler,
        IDomainNotificationHandler<DomainNotification> notifications) : base(notifications) 
    {
        _query = query;
        _handler = handler;
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

    // [HttpPost]
    // public async Task<IActionResult> Post([FromBody] AdicionarModeloCommand command)
    // {
    //     await _bus.EnviarComando(command);
    //     return Ok("Modelo criado");
    // }
    // Novo com o Query
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] AdicionarModeloCommand command)
    {
        // 1. Executa a lógica
        await _handler.Handle(command);

        // 2. Usa o método mágico da sua BaseController!
        // Se houver erro, o método Response já retorna BadRequest automaticamente.
        // Se estiver ok, ele retorna o ID gerado.
        return await Response(new { id = command.IdGerado });
    }

    [HttpGet("dropdown")]
    public async Task<IActionResult> ObterModelosParaDropdown()
    {
        var modelos = await _query.ObterModelosParaDropdown();
        return Ok(modelos);
    }
}
