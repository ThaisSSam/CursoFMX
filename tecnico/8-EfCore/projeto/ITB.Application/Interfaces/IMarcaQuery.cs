using ITB.Application.Dtos;

namespace ITB.Application.Interfaces;

public interface IMarcaQuery
{
    Task<IEnumerable<MarcaComVeiculoDto>> ObterMarcasComVeiculos();
    Task<IEnumerable<MarcaComVeiculoDto>> ObterMarcasComVeiculosProjecao();
}
