using System;
using LojaApi.Entities;

namespace LojaApi.Infra.Repositories.Interfaces;

public interface IEnderecoRepository
{
    Endereco Adicionar(Endereco novoEndereco);
    Endereco? ObterPorId(int id);
    List<Endereco> ObterTodos();
}
