using System;
using LojaApi.Entities;
using LojaApi.Repositories.Interfaces;
using LojaApi.Services.Interfaces;

namespace LojaApi.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;
        // O Service agora recebe sua dependência (o contrato do repositório) via construtor.
        public ClienteService(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        // Os métodos agora usam a dependência injetada (_clienteRepository)
        public List<Cliente> GetAll()
        {
            // Regra: Não exibir clientes inativos.
            return _clienteRepository.GetAll().Where(c => c.Ativo).ToList();
        }

        public Cliente? GetById(int id)
        {
            return _clienteRepository.GetById(id);
        }

        public Cliente Add(Cliente novoCliente)
        {
            novoCliente.Nome = novoCliente.Nome.ToUpper();
            novoCliente.Ativo = true;
            return _clienteRepository.Add(novoCliente);
        }

        public Cliente? Update(int id, Cliente clienteAtualizado)
        {
            if (id != clienteAtualizado.Id) return null;
            return _clienteRepository.Update(id, clienteAtualizado);
        }

        public bool Delete(int id)
        {
            var cliente = _clienteRepository.GetById(id);
            if (cliente != null)
            {
                cliente.Ativo = false;
                return _clienteRepository.Update(id, cliente) != null;
            }
            return false;
        }
    }
}