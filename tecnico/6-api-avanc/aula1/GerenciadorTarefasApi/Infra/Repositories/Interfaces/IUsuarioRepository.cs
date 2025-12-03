using GerenciadorTarefasApi.Entities;
using System.Collections.Generic;

namespace GerenciadorTarefasApi.Infra.Repositories.Interfaces
{
    public interface IUsuarioRepository
    {
        List<Usuario> ObterTodos();
        Usuario? ObterPorId(int id, bool incluirTarefas = false);
        Usuario Adicionar(Usuario novoUsuario);
        Usuario? Atualizar(Usuario usuario);
        void Deletar(Usuario usuario);
        bool SalvarAlteracoes();
    }
}