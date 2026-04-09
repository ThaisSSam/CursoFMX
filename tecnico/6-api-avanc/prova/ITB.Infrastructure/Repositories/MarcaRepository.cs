using System.Threading.Tasks;
using ITB.Domain.Entities;
using ITB.Domain.Interfaces;
using ITB.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ITB.Infrastructure.Repositories;

public class MarcaRepository : RepositoryBase<Marca>, IMarcaRepository
{
    public MarcaRepository(AppDbContext context) : base(context) { }

    public async Task<bool> VerificarExistencia(int id)
    {
        // Verifique se o nome da tabela no seu AppDbContext é 'marcas' ou 'Marcas'
        return await _context.marcas.AnyAsync(x => x.Id == id);
    }

    // Você também precisa implementar este, senão dará erro de novo:
    public async Task<bool> PossuiModelosAtivos(int marcaId)
    {
        // Exemplo de lógica simples, ajuste conforme sua regra
        return await _context.Modelos.AnyAsync(m => m.MarcaId == marcaId);
    }
}