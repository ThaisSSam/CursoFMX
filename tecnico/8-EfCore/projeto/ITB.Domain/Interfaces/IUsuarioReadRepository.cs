using System;
using ITB.Domain.Entities;

namespace ITB.Domain.Interfaces;

public interface IUsuarioReadRepository 
{ 
    // Usado no Login e para garantir que não há e-mails duplicados 
    Task<Usuario?> ObterPorEmailAsync(string email); 
    Task<Usuario?> ObterPorIdAsync(int id); 
}