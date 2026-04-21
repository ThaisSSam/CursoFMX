using System.Security.Claims;
using ITB.API.Controllers.Base;
using ITB.Application.Commands;
using ITB.Application.Dtos;
using ITB.Application.Interfaces;
using ITB.Application.Queries;
using ITB.Domain.Core.Messages;
using ITB.Domain.Core.Messages.Interfaces;
using ITB.Domain.Entities;
using ITB.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.AspNetCore.RateLimiting;

namespace ITB.API.Controllers;

[Route("api/[controller]")]
[ApiKey]
[Authorize]
[EnableRateLimiting(("PadraoPorIp"))]
public class VeiculoController : BaseController
{
    private readonly IVeiculoRepository _repository;
    private readonly IMessageBus _bus;
    private readonly IVeiculoQuery _query;
    private readonly IConfiguration _configuration;

    private readonly ExportarVeiculosExcelQueryHandler _handler;

    public VeiculoController(IVeiculoRepository repository, IMessageBus bus, IVeiculoQuery query, IConfiguration configuration,
        ExportarVeiculosExcelQueryHandler handler,
        IDomainNotificationHandler<DomainNotification> notifications) : base(notifications)
    {
        _repository = repository;
        _bus = bus;
        _query = query;
        _configuration = configuration;
        _handler = handler;
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


    [HttpGet("check")]
    // [DisableRateLimiting]
    public ActionResult Check([FromHeader(Name="X-Super-Token")] string superToken)
    {
        HttpContext.Request.Headers.TryGetValue("X-Super-Token", out var superTokenHeader);
        var superTokenValido = _configuration.GetValue<string>("Seguranca:ChaveSuporte");

        if (superTokenValido != superTokenHeader)
        {
            return Unauthorized("Super Token inválido");
        }

        return Ok("API Online");
    }

    [HttpGet("relatorio-ultimos-dez-veiculos")]
    public async Task<ActionResult<VeiculoReadDto>> GetRelatorioUltimosDezVeiculos()
    {
        var veiculos = await _query.RelatorioUltimosVeiculosRegistrados(10);

        if (veiculos.Count() == 0)
            return NotFound(new { message = "Nenhum veículo registrado." });

        return Ok(veiculos);
    }

    [HttpGet("catalogo-feirao-offset")]
    public async Task<ActionResult<VeiculoReadDto>> GetCatalogoFeiraoOffset([FromQuery] int numeroPagina, int tamanhoPagina)
    {
        var veiculos = await _query.ObterVeiculosOffsetAsync(numeroPagina, tamanhoPagina);

        if (veiculos.Count() == 0)
            return NotFound(new { message = "Nenhum veículo registrado." });

        return Ok(veiculos);
    }

    [HttpGet("catalogo-feirao-keyset")]
    public async Task<ActionResult<VeiculoReadDto>> GetCatalogoFeiraoKeyset([FromQuery] int ultimoIdDaPagina, int tamanhoPagina)
    {
        var veiculos = await _query.ObterVeiculosKeysetAsync(ultimoIdDaPagina, tamanhoPagina);

        if (veiculos.Count() == 0)
            return NotFound(new { message = "Nenhum veículo registrado." });

        return Ok(veiculos);
    }

    [HttpGet("relatorio-estoque")]
    public async Task<IActionResult> GetRelatorioEstoque()
    {
        var veiculos = await _query.RelatorioModeloEstoque();

        return Ok(veiculos);
    }

    [HttpGet]
    [AllowAnonymous]
    [OutputCache(Duration = 30)]
    public async Task<IActionResult> Get()
    {
        await Task.Delay(5000);
 
        var veiculos = await _query.ObterTodos();

        return Ok(veiculos);

        // var veiculosDTO = veiculos.Select(v => new VeiculoListagemDto
        // {
        //     Id = v.Id,
        //     Placa = v.Placa,
        //     Ano = v.Ano,
        //     Ativo = v.Ativo,

        //     Marca = v.Marca != null ? new MarcaDTO
        //     {
        //         Id = v.Marca.Id,
        //         Nome = v.Marca.Nome,
        //         Ativo = v.Marca.Ativo
        //     } : null,

        //     Modelo = v.Modelo != null ? new ModeloDTO
        //     {
        //         Id = v.Modelo.Id,
        //         MarcaId = v.Modelo.MarcaId,
        //         Nome = v.Modelo.Nome,
        //         Marca = new MarcaDTO
        //         {
        //             Id = v.Modelo.Marca.Id,
        //             Nome = v.Modelo.Marca.Nome,
        //             Ativo = v.Modelo.Marca.Ativo
        //         },
        //         Ativo = v.Modelo.Ativo
        //     } : null
        // });

        //return Ok(veiculosDTO);
    }

    // [HttpGet("{id}")]
    // public async Task<ActionResult<VeiculoReadDto>> GetById(int id)
    // {
    //     var veiculo = await _repository.ObterPorIdAsync(id);

    //     if (veiculo == null)
    //         return NotFound(new { message = "Veículo não encontrado." });

    //     return Ok(veiculo);
    // }

    [HttpPost]
    [EnableRateLimiting(("RigidaPorIp"))]

    public async Task<ActionResult> Post([FromBody] AdicionarVeiculoCommand command)
    {
        await _bus.EnviarComando(command);
        if (command.IdGerado is null)
            await _notifications.Handle(new DomainNotification("Id", "Erro ao dicionar veículo."));

        return Ok(new { id = command.IdGerado, mensagem = "Veículo adicionado com sucesso" });
    }

    // [HttpPost("desconto-por-marca")]
    // public async Task<ActionResult> DescontoPorMarca([FromBody] DescontoMarcaVeiculoCommand command)
    // {
    //     var resultado = await _bus.EnviarComando(command);
    //     if (!resultado.Sucesso)
    //         return BadRequest(resultado);

    //     return Ok(new { QtdAlterada = resultado.Dados, mensagem = resultado.Mensagem });
    // }

    // [HttpDelete("{id}")]
    // public async Task<ActionResult<VeiculoReadDto>> DeleteById(int id)
    // {
    //     var veiculo = await _repository.ObterPorIdAsync(id);

    //     if (veiculo == null)
    //         return NotFound(new { message = "Veículo não encontrado." });

    //     await _bus.EnviarComando(new DeletarVeiculoCommand
    //     {
    //         Id = id
    //     });

    //     return NoContent();
    // }

    // [HttpPut("{id}")]
    // public async Task<IActionResult> Put(int id, [FromBody] AtualizarVeiculoCommand command)
    // {
    //     if (id != command.Id) return BadRequest(new { mensagem = "IDs divergentes." });

    //     await _bus.EnviarComando(command);
    //     return Ok(new { mensagem = "Veículo atualizado com sucesso." });
    // }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] AtualizarVeiculoRequest request)
    {
        var command = request.ToCommand(id);

        await _bus.EnviarComando(command);

        return await Response(new { Id = command.IdGerado });
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Gerente")]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new DesativarVeiculoCommand { Id = id };

        await _bus.EnviarComando(command);
        return Ok(new { mensagem = "Veículo desativado com sucesso." });
    }

    [HttpGet("obter-todos-mapping")]
    public async Task<IActionResult> GetMapping()
    {
        // 1. A Query busca os dados no banco já otimizados
        // Nota: Como a interface já retorna Task<IEnumerable<VeiculoListagemDTO>>, 
        // se a sua Query já fizer o mapeamento direto no SQL (ex: Dapper), você nem precisa do Mapper aqui!
        // Mas se a Query retornar as Entidades (IEnumerable<Veiculo>), usamos o Mapper abaixo:

        var veiculos = await _query.ObterTodos();

        // 2. Se a Query não devolver o DTO pronto, usamos nosso Extension Method:
        // var veiculosDto = veiculos.ToListagemDtoList();

        // 3. Devolve envelopado
        return await Response(veiculos);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var veiculo = await _query.ObterPorId(id);

        if (veiculo == null)
        {
            await _notifications.Handle(new DomainNotification("Id", "Veículo não encontrado."));
            return await Response();
        }

        return await Response(veiculo);
    }


    [HttpPost("novo")]
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

}