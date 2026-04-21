using System;
using ITB.Domain.Entities;
using ITB.Domain.Interfaces;
using ITB.Infrastructure.Persistence;

namespace ITB.Infrastructure.Repositories;

public class UsuarioWriteRepository(AppDbContext context) : IUsuarioWriteRepository 
{ 
    public async Task AdicionarAsync(Usuario usuario) => await 
context.Usuarios.AddAsync(usuario); 
    public void Atualizar(Usuario usuario) => context.Usuarios.Update(usuario); 
}