using ITB.Domain.Entities;
using ITB.Domain.Interfaces;
using ITB.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ITB.Infrastructure.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly AppDbContext _context;
    public UsuarioRepository(AppDbContext context) => _context = context;

    public async Task<Usuario?> ObterPorEmailESenha(string email, string senha)
    {
        return await _context.Usuarios
            .FirstOrDefaultAsync(u => u.email == email && u.senha == senha);
    }
}