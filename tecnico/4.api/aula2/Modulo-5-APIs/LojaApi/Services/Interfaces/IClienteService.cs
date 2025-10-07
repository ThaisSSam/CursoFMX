using System;
using LojaApi.Entities;

namespace LojaApi.Services.Interfaces
{
    public interface IClienteService
    {
        List<Cliente> GetAll();
        Cliente? GetById(int id);
        Cliente Add(Cliente novoCliente);
        Cliente? Update(int id, Cliente clienteAtualizado);
        bool Delete(int id);
    }
        

}