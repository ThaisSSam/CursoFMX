using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging; // 👈 Necessário para os seletores de log do seu contexto
using System.Threading.Tasks;
using Treinamento.Domain.Aggregates.Usuarios;
using Treinamento.Infrastructure.Persistence; // 👈 Namespace exato do seu TreinamentoContext!

namespace Treinamento.Infrastructure.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
  private readonly TreinamentoContext _context;

  // Construtor ajustado para receber o contexto que mapeia o WriteContext
  public UsuarioRepository(TreinamentoContext context)
  {
    _context = context;
  }

  public async Task<Usuario?> ObterPorEmailAsync(string email)
  {
    // Busca o usuário filtrando pelo e-mail
    return await _context.Usuarios
        .FirstOrDefaultAsync(u => u.Email == email);
  }

  public async Task AtualizarAsync(Usuario usuario)
  {
    // Atualiza a entidade no contexto de escrita
    _context.Usuarios.Update(usuario);
    await _context.SaveChangesAsync();
  }

}