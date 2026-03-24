using System;
using ITB.API.Filters;
using ITB.Application.Commands;
using ITB.Application.Dtos;
using ITB.Application.Interfaces;
using ITB.Domain.Core.Commands;
using ITB.Domain.Core.Messages.Interfaces;
using ITB.Domain.Interfaces;
using ITB.Infrastructure.Persistence;
using ITB.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITB.API.Controller;

[ApiController]
[Route("api/[controller]")]

public class VeiculoController : ControllerBase
{
    private readonly IMessageBus _bus;

    private readonly IVeiculoQuery _query;

    private readonly AppDbContext _context;

    private readonly IVeiculoRepository _veiculoRepository;

    public VeiculoController(IMessageBus bus, AppDbContext context, IVeiculoRepository veiculoRepository, IVeiculoQuery query)
    {
        _bus = bus;
        _context = context;
        _veiculoRepository = veiculoRepository;
        _query = query;
    }

    // LEITURA: Direto do repositório
    [HttpGet]
    // public async Task<IActionResult> Get()
    // {
    //     var veiculos = await _veiculoRepository.ObterTodos();

    //     var veiculosDTO = veiculos.Select(v => new VeiculoDTO
    //     {
    //         Id = v.Id,            
    //         Placa = v.Placa,
    //         Ano = v.Ano,
    //         Ativo = v.Ativo,

    //         // Mapeamento do objeto aninhado:
    //         // Marca = v.Marca != null ? new MarcaDTO
    //         // {
    //         //     Id = v.Marca.Id,
    //         //     Nome = v.Marca.Nome,                
    //         // } : null

    //         Modelo = v.Modelo != null ? new ModeloDTO
    //         {
    //             Id = v.Modelo.Id, 
    //             Nome = v.Modelo.Nome,
    //             Ativo = v.Modelo.Ativo
    //         } : null
    //     });

    //     return Ok(veiculosDTO);
    // }

    // Novo com o Query
    [AllowAnonymous]
    public async Task<IActionResult> ObterTodosAsync()
    {
        var veiculos = await _query.ObterTodosAsync();
        return Ok(veiculos);
    }

    [HttpPost]
    // public async Task<IActionResult> Post([FromBody]AdicionarVeiculoCommand command)
    // {
    //     await _bus.EnviarComando(command);
    //     return Ok(new{mensagem= "Veículo criado"});
    // }

    // Novo com o Query
    public async Task<IActionResult> Adicionar([FromBody] AdicionarVeiculoCommand command)
    {
        await _bus.EnviarComando(command);
        return Ok(new { mensagem = "Veículo enviado para processamento." });
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

    [HttpGet("dez-ultimos")]
    [ProducesResponseType(typeof(IEnumerable<DezUltimosVeiculosDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDezUltimos()
    {
        var resultado = await _query.DezUltimosComVeiculosAsync();
        return Ok(resultado);
    }

    [HttpGet("desconto")]
    [ProducesResponseType(typeof(IEnumerable<VeiculoRelatorioDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDesconto([FromQuery] string marcaNome)
    {
        var resultado = await _query.AplicarDescontoAsync(marcaNome);
        return Ok(resultado);
    }

    // MISSÃO 1 - Passo A
    [HttpGet("paginacao-offset")]
    public async Task<IActionResult> GetPaginacaoOffset([FromQuery] int pagina = 1, [FromQuery] int tamanho = 20)
    {
        // O retorno agora contém Dados, PaginaAtual, TotalRegistros e TotalPaginas
        var resultado = await _query.ObterVeiculosOffsetAsync(pagina, tamanho);
        return Ok(resultado);
    }

    [HttpGet("paginacao-keyset")]
    public async Task<IActionResult> GetPaginacaoKeyset([FromQuery] int? ultimoId, [FromQuery] int tamanho = 20)
    {
        // O retorno contém Dados, TemMaisPaginas e ProximoCursor
        var resultado = await _query.ObterVeiculosKeysetAsync(ultimoId, tamanho);
        return Ok(resultado);
    }

    [HttpGet("check")]
    public async Task<IConfiguration> GetCheck()
    {
        
        return Ok("Api online");
    }
}
