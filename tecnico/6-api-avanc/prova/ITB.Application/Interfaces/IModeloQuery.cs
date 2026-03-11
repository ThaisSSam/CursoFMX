using System;
using ITB.Application.Dtos;
using ITB.Domain.Entities;

namespace ITB.Application.Interfaces;
// 5. para definir o contrato do que sua aplicação pode pedir dps vem as queries
public interface IModeloQuery
{
    Task<IEnumerable<RelatorioModeloDTO>> ObterTodosAsync(); 
    Task<RelatorioModeloDTO?> ObterPorIdAsync(int id); 

    Task<IEnumerable<ModeloDropdownDTO>> ObterModelosParaDropdown();
}
