using System;
using ApiEstoque.Entities;
using ApiEstoque.Infra.DTOs;

namespace ApiEstoque.Services.Interfaces;

public interface IFabricanteService
{
    Task<List<FabricanteDto>> ObterTodosFabricanteAsync();

    Task<FabricanteDto?> ObterFabricantePorIdAsync(int id);

    Task<FabricanteDto> AdicionarFabricanteAsync(CriarFabricanteDto fabricanteDto);

    Task <FabricanteDto> DeletarFabricanteAsync(int id);
}
