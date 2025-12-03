using GerenciadorTarefasApi.Entities;
using System.Collections.Generic;

namespace GerenciadorTarefasApi.Infra.Repositories.Interfaces
{
    public interface ITagRepository
    {
        List<Tag> ObterTodos();
        Tag? ObterPorId(int id);
        Tag Adicionar(Tag novaTag);
        Tag? Atualizar(Tag tag);
        void Deletar(Tag tag);
        bool SalvarAlteracoes();
    }
}