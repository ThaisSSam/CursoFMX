using System;
using LojaApi.Entities;

namespace LojaApi.Repositories.Interfaces;

public interface IClienteRepository
{
    List<Cliente> GetAll();
    Cliente? GetById(int id);
    Cliente Add(Cliente novoCliente);
    Cliente? Update(int id, Cliente clienteAtualizado);
}
