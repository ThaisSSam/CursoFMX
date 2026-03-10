using System;
using ITB.Application.Dtos;
using ITB.Infrastructure.Queries;

namespace ITB.Application.Interfaces;

public interface IMarcaQuery
{
    Task<IEnumerable<MarcaComVeiculosDTO>> ObterMarcasComVeiculosAsync();
}
