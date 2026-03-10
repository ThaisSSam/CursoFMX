using System;
using ITB.Application.Dtos;
using ITB.Application.Interfaces;
using ITB.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ITB.Infrastructure.Queries;

public class ModeloEMarca : IModeloQuery
{
    private readonly AppDbContext _context;

    public ModeloEMarca(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ModeloDropdownDTO>> ObterModelosParaDropdown()
    {
        var query = _context.Database.SqlQuery<ModeloDropdownDTO>($@"
            SELECT
                m.id,
                m.nome,
                mar.nome AS marca 
            FROM modelo m 
            INNER JOIN modelos 
        ");
    }
}
