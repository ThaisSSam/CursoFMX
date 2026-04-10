using ITB.Application.Dtos;

namespace ITB.Application.Interfaces;

public interface IModeloQuery
{
    Task<IEnumerable<ModeloDropdownDto>> ObterModelosParaDropdown();
}
