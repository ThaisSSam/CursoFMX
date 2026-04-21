using System;
using ITB.Domain.Entities;

namespace ITB.Domain.Interfaces;

public interface IUsuarioWriteRepository 
{ 
    Task AdicionarAsync(Usuario usuario); 
    void Atualizar(Usuario usuario); 
}