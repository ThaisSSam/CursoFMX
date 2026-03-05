using System;
using ITB.Application.Dtos;

namespace ITB.Application.Interfaces;

public interface IVeiculoQuery 
{ 
    Task<IEnumerable<VeiculosListagemDTO>> ObterTodosAsync(); 
    Task<VeiculosListagemDTO?> ObterPorIdAsync(int id); 
}
