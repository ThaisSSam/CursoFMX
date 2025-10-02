using System;
using ApiTeste.Entities;

namespace ApiTeste.Repositories;

public class ClienteRepository
{
    public Cliente GetCliente()
    {
        return new Cliente  { Id = 1, Nome = "John Doe", Email = "john@gmail.com" };
    }
}
