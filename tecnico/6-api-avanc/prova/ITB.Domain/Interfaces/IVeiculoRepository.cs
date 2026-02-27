using System;
using System.Threading.Tasks;
using ITB.Domain.Entities;

namespace ITB.Domain.Interfaces;
public interface IVeiculoRepository
{
    Task AdicionarAsync(Veiculo veiculo);
    Task Atualizar(Veiculo veiculo);
    Task<IEnumerable<Veiculo>> ObterTodos();
    Task<Veiculo?> ObterPorId(int id);
    Task<bool> PlacaJaExiste(string placa, int veiculoIdIgnorado);
    
}