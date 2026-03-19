using System;
using ITB.Application.Dtos;
using ITB.Infrastructure.Queries;

namespace ITB.Application.Interfaces;

public interface IVeiculoQuery 
{ 
    Task<IEnumerable<VeiculosListagemDTO>> ObterTodosAsync(); 
    Task<VeiculosListagemDTO?> ObterPorIdAsync(int id); 

    Task<IEnumerable<MarcaComVeiculosDTO>> ObterMarcasComVeiculosAsync();

    Task<IEnumerable<DezUltimosVeiculosDTO>> DezUltimosComVeiculosAsync();

    Task<IEnumerable<VeiculoRelatorioDTO>> AplicarDescontoAsync(string nomeMarca);

    Task<PaginacaoOffsetResponse<VeiculoRelatorioDTO>> ObterVeiculosOffsetAsync(int numeroPagina, int tamanhoPagina);
    Task<PaginacaoResponse<VeiculoRelatorioDTO>> ObterVeiculosKeysetAsync(int? ultimoIdDaPagina, int tamanhoPagina);  
}
