using System;
using ITB.Application.Dtos;

namespace ITB.Application.Interfaces;

public interface IFabricanteService
{
    Task<IEnumerable<FabricanteReadDto>> ObterTodosAsync();
    Task<FabricanteReadDto?> ObterPorIdAsync(int id);
    Task<FabricanteReadDto> AdicionarAsync(FabricanteCreateDto dto);
    Task<bool> AtualizarAsync(FabricanteUpdateDto dto);
    Task<bool> RemoverAsync(int id);
}
