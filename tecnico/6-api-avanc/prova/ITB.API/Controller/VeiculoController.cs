using System;
using System.Security.Claims;
using ITB.API.Controller.Base;
using ITB.API.Filters;
using ITB.Application.Commands;
using ITB.Application.Dtos;
using ITB.Application.Handlers;
using ITB.Application.Interfaces;
using ITB.Application.Mappings;
using ITB.Application.Queries;
using ITB.Domain.Core.Commands;
using ITB.Domain.Core.Messages.Interfaces;
using ITB.Domain.Core.Notifications;
using ITB.Domain.Interfaces;
using ITB.Infrastructure.Persistence;
using ITB.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;

namespace ITB.API.Controller;

[ApiController]
[Route("api/[controller]")]
[Authorize]
// [EnableRateLimiting("PoliticaPadrao")]
public class VeiculoController : BaseController
{
    private readonly ExportarVeiculosExcelQueryHandler _handler;
    private readonly IMessageBus _bus;

    private readonly IVeiculoQuery _query;

    private readonly AppDbContext _context;

    private readonly IVeiculoRepository _veiculoRepository;

    public VeiculoController(IMessageBus bus, AppDbContext context, IVeiculoRepository veiculoRepository, IVeiculoQuery query, ExportarVeiculosExcelQueryHandler exportarExcel,
    IDomainNotificationHandler<DomainNotification> notifications):base(notifications)
    {
        _bus = bus;
        _context = context;
        _veiculoRepository = veiculoRepository;
        _query = query;
        _handler = exportarExcel;
        
    }

    [HttpGet("exportar-excel")]
    public async Task<IActionResult> ExportarExcel()
    {
        var cargo = User.FindFirstValue(ClaimTypes.Role) ?? "Cliente";

        var query = new ExportarVeiculosExcelQuery { CargoUsuarioLogado = cargo };

        var arquivoDto = await _handler.Handle(query);

        // O método File() pega nossos bytes e empacota no formato exato que o HTTP exige para downloads.
        return File(arquivoDto.Conteudo, arquivoDto.ContentType, arquivoDto.NomeArquivo);
    }


    // LEITURA: Direto do repositório
    // [HttpGet]
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
    // [AllowAnonymous]
    // [OutputCache(Duration =30)]
    // public async Task<IActionResult> ObterTodosAsync()
    // {
    //     var veiculos = await _query.ObterTodosAsync();
    //     return Ok(veiculos);
    // }

    [HttpGet("obter-todos-mapping")]
    public async Task<IActionResult> GetMapping()
    {
        // 1. A Query busca os dados no banco já otimizados
        // Nota: Como a interface já retorna Task<IEnumerable<VeiculoListagemDTO>>, 
        // se a sua Query já fizer o mapeamento direto no SQL (ex: Dapper), você nem precisa do Mapper aqui!
        // Mas se a Query retornar as Entidades (IEnumerable<Veiculo>), usamos o Mapper abaixo:

        var veiculos = await _query.ObterTodosAsync();

        // 2. Se a Query não devolver o DTO pronto, usamos nosso Extension Method:
        // var veiculosDto = veiculos.ToListagemDtoList();

        // 3. Devolve envelopado
        return await Response(veiculos);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var veiculo = await _query.ObterPorIdAsync(id);

        if (veiculo == null)
        {
            await _notifications.Handle(new DomainNotification("Id", "Veículo não encontrado."));
            return await Response();
        }

        return await Response(veiculo);
    }

    // [HttpGet]
    // [AllowAnonymous]
    // [OutputCache(Duration = 30)]
    // public async Task<IActionResult> Get()
    // {
    //     // PROVA 1: O Terminal
    //     // Se o cache funcionar, essa frase NÃO PODE aparecer nas próximas chamadas.
    //     Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] => Fui no Banco de Dados buscar os veículos!");

    //     // PROVA 2: A Performance (Simulando uma query pesada)
    //     // Vamos atrasar o código em 3 segundos de propósito.
    //     await Task.Delay(5000);

    //     var veiculos = await _query.ObterTodosAsync(); // Sua busca real no banco

    //     // PROVA 3: O Relógio Congelado no JSON
    //     // Vamos envelopar a resposta para mostrar a hora exata em que o JSON foi montado.
    //     return Ok(new
    //     {
    //         CacheGeradoEm = DateTime.Now.ToString("HH:mm:ss"), // Esse valor vai congelar!
    //         TotalVeiculos = veiculos.Count(), // (Opcional) Ajuste conforme sua lista
    //         Dados = veiculos
    //     });
    // }

    // [HttpPost]
    // [EnableRateLimiting("PoliticaPorIp")]
    // public async Task<IActionResult> Post([FromBody]AdicionarVeiculoCommand command)
    // {
    //     await _bus.EnviarComando(command);
    //     return Ok(new{mensagem= "Veículo criado"});
    // }

    // Novo com o Query
    // public async Task<IActionResult> Adicionar([FromBody] AdicionarVeiculoCommand command)
    // {
    //     await _bus.EnviarComando(command);
    //     return Ok(new { mensagem = "Veículo enviado para processamento." });
    // }

    /// <summary>
    /// ESCRITA: Cadastra um novo veículo.
    /// Aqui sim usamos o Bus e o Request DTO para blindar a aplicação.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] AdicionarVeiculoRequest request)
    {
        // 1. Transformamos o JSON da Web na Intenção de Negócio (Command) com segurança
        var command = request.ToCommand();

        // 2. Disparamos a intenção. A Controller não valida placa nem preço, o Handler resolve isso!
        await _bus.EnviarComando(command);

        // 3. Deixamos a BaseController decidir o que responder!
        // Se o Handler gerou erros (ex: placa duplicada), a BaseController ignora o ID gerado e devolve os erros.
        // Se deu tudo certo, a BaseController devolve um Status 200 com o ID gerado dentro da propriedade "Data".
        return await Response(new { Id = command.IdGerado });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] AtualizarVeiculoCommand command)
    {
        if (id != command.Id) return BadRequest(new { mensagem = "IDs divergentes." });

        await _bus.EnviarComando(command);
        return Ok(new { mensagem = "Veículo atualizado com sucesso." });
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Gerente")]
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
    [OutputCache(Duration = 30)]
    [ProducesResponseType(typeof(IEnumerable<VeiculoRelatorioDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDesconto([FromQuery] string marcaNome)
    {
        var resultado = await _query.AplicarDescontoAsync(marcaNome);
        return Ok(resultado);
    }

    // MISSÃO 1 - Passo A
    [HttpGet("paginacao-offset")]
    [OutputCache(Duration = 30)]
    public async Task<IActionResult> GetPaginacaoOffset([FromQuery] int pagina = 1, [FromQuery] int tamanho = 20)
    {
        // O retorno agora contém Dados, PaginaAtual, TotalRegistros e TotalPaginas
        var resultado = await _query.ObterVeiculosOffsetAsync(pagina, tamanho);
        await Task.Delay(5000);
        return Ok(resultado);
    }

    [HttpGet("paginacao-keyset")]
    [EnableRateLimiting("PadraoPorIp")]
    public async Task<IActionResult> GetPaginacaoKeyset([FromQuery] int? ultimoId, [FromQuery] int tamanho = 20)
    {
        // O retorno contém Dados, TemMaisPaginas e ProximoCursor
        var resultado = await _query.ObterVeiculosKeysetAsync(ultimoId, tamanho);
        return Ok(resultado);
    }

    // [HttpGet("check")]
    // public async Task<IConfiguration> GetCheck()
    // {        
    //     // return Ok("Api online");
    // }
}
