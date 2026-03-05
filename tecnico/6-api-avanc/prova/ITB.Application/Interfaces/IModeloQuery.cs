using System;
using ITB.Application.Dtos;
using ITB.Domain.Entities;

namespace ITB.Application.Interfaces;

public interface IModeloQuery
{
    Task<IEnumerable<RelatorioModeloDTO>> ObterTodosAsync(); 
    Task<RelatorioModeloDTO?> ObterPorIdAsync(int id); 
}
