using ITB.Application.Dtos;

namespace ITB.Application.Interfaces;

public interface IVeiculoQuery
{
    Task<IEnumerable<VeiculoListagemDto>> ObterTodos();
    Task<IEnumerable<VeiculoExportacaoExcelDto>> ObterComLucro();
    Task<VeiculoListagemDto?> ObterPorId(int id);
    Task<IEnumerable<RelatorioModeloDto>> RelatorioModeloEstoque();
    Task<IEnumerable<DashboardVeiculoDTO>> RelatorioUltimosVeiculosRegistrados(int qtdMaxima);
    Task<IEnumerable<VeiculoListagemDto>> ObterVeiculosOffsetAsync(int numeroPagina, int tamanhoPagina);
    Task<IEnumerable<VeiculoListagemDto>> ObterVeiculosKeysetAsync(int? ultimoIdDaPagina, int tamanhoPagina);
}
