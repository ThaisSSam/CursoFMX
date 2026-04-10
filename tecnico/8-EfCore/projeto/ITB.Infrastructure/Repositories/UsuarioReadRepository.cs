using System;
using ITB.Domain.Entities;
using ITB.Domain.Interfaces;
using ITB.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ITB.Infrastructure.Repositories;

public class UsuarioReadRepository(AppDbContext context) : IUsuarioReadRepository 
{ 
    public async Task<Usuario?> ObterPorEmailAsync(string email) 
    { 
        // AsNoTracking() é crucial aqui para máxima performance de leitura 
        return await context.Usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.Email 
== email); 
    } 
 
    public async Task<Usuario?> ObterPorIdAsync(int id) 
    { 
        return await context.Usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.Id == 
id); 
    } 
}
