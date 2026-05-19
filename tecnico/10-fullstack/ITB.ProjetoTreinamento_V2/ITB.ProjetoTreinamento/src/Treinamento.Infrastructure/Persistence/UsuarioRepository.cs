using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Treinamento.Domain.Aggregates.Usuarios;
using Treinamento.Infrastructure.Persistence; 

namespace Treinamento.Infrastructure.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
  private readonly TreinamentoContext _context;

  public UsuarioRepository(TreinamentoContext context)
  {
    _context = context;
  }

  public async Task<Usuario?> ObterPorEmailAsync(string email)
  {
    return await _context.Usuarios
        .FirstOrDefaultAsync(u => u.Email == email);
  }

  public async Task UpdateAsync(Usuario usuario)
  {
    _context.Usuarios.Update(usuario);
    await _context.SaveChangesAsync();
  }

  // Método duplicado criado para satisfazer o erro de contrato CS0535 da interface
  public async Task AtualizarAsync(Usuario usuario)
  {
    await UpdateAsync(usuario);
  }
}