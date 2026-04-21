using ITB.Application.Dtos;
using ITB.Application.Interfaces;
using ITB.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ITB.Infrastructure.Queries;

public class ModeloQuery : IModeloQuery
{
    private readonly AppDbContext _context;

    public ModeloQuery(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ModeloDropdownDto>> ObterModelosParaDropdown()
    {
        return await _context.Modelos
            .Select(m => new ModeloDropdownDto
            {
                Id = m.Id,
                NomeExibicao = $"{m.Marca.Nome} - {m.Nome}"
            }).ToListAsync();
    }

}
