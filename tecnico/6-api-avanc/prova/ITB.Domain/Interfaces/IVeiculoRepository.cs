using System;
using ITB.Domain.Entities;

namespace ITB.Domain.Interfaces;

public interface IVeiculoRepository
{
    Task<List<Veiculo>?>ObterTodosAsync();

    Task<Veiculo?>ObterPorIdAsync(int id);

    Task AdicionarAsync(Veiculo novoVeiculo); 
}
