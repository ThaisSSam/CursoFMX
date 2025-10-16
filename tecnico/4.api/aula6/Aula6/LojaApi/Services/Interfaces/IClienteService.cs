using System;
using LojaApi.Entities;
using LojaApi.Infra.DTOs;

namespace LojaApi.Services.Interfaces;

public interface IClienteService
{
    List<Cliente> ObterTodos();
    Cliente? ObterPorId(int id);
    Cliente? Atualizar(int id, Cliente clienteAtualizado);
    bool Remover(int id);
    Cliente Adicionar(CriarClienteDto clienteDto);
    ClienteDetalhadoDto? ObterDetalhesPorId(int id);
}
