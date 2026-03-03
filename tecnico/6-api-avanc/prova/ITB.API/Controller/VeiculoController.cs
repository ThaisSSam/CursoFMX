using System;
using ITB.Application.Commands;
using ITB.Application.Dtos;
using ITB.Domain.Core.Commands;
using ITB.Domain.Core.Messages.Interfaces;
using ITB.Domain.Interfaces;
using ITB.Infrastructure.Persistence;
using ITB.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITB.API.Controller;

[ApiController]
[Route("api/[controller]")]
public class VeiculoController : ControllerBase
{
    private readonly IMessageBus _bus;

    private readonly AppDbContext _context;

    private readonly IVeiculoRepository _veiculoRepository;

    public VeiculoController(IMessageBus bus, AppDbContext context, IVeiculoRepository veiculoRepository)
    {
        _bus = bus;
        _context = context;
        _veiculoRepository = veiculoRepository;
    }

    // LEITURA: Direto do repositório
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var veiculos = await _veiculoRepository.ObterTodos();

        var veiculosDTO = veiculos.Select(v => new VeiculoDTO
        {
            Id = v.Id,            
            Placa = v.Placa,
            Ano = v.Ano,
            Ativo = v.Ativo,

            // Mapeamento do objeto aninhado:
            // Marca = v.Marca != null ? new MarcaDTO
            // {
            //     Id = v.Marca.Id,
            //     Nome = v.Marca.Nome,                
            // } : null

            Modelo = v.Modelo != null ? new ModeloDTO
            {
                Id = v.Modelo.Id, 
                Nome = v.Modelo.Nome,
                Ativo = v.Modelo.Ativo
            } : null
        });

        return Ok(veiculosDTO);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody]AdicionarVeiculoCommand command)
    {
        await _bus.EnviarComando(command);
        return Ok(new{mensagem= "Veículo criado"});
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] AtualizarVeiculoCommand command)
    {
        if (id != command.Id) return BadRequest(new { mensagem = "IDs divergentes." });

        await _bus.EnviarComando(command);
        return Ok(new { mensagem = "Veículo atualizado com sucesso." });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new DesativarVeiculoCommand { Id = id };

        await _bus.EnviarComando(command);
        return Ok(new { mensagem = "Veículo desativado com sucesso." });
    }
}
