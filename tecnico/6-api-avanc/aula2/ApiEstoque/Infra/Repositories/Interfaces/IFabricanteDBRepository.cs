using System;
using ApiEstoque.Entities;

namespace ApiEstoque.Infra.Repositories.Interfaces;

public interface IFabricanteDBRepository
{
    Task<List<Fabricante>> ObterTodosFabricanteAsync();

    Task<Fabricante> ObterFabricantePorIdAsync(int id);

    Task <Fabricante> AdicionarFabricanteAsync(Fabricante novoFabricante);

    Task<Fabricante> AtualizarFabricanteAsync(Fabricante fabricante);

    Task DeletarFabricanteAsync(Fabricante fabricante);
}
