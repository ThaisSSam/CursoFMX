using ITB.Domain.Entities;

namespace ITB.Domain.Interfaces;

public interface IVeiculoRepository : IRepositoryBase<Veiculo>
{
    Task<bool> PlacaJaExiste(string placa, int veiculoIdIgnorado);
    Task<int> DescontoEmMassaPorMarca(string marca, decimal percentualDesconto, int ano);

}
