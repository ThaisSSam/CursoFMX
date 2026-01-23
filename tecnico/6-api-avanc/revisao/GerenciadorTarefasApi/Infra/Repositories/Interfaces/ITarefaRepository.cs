using System;
using GerenciadorTarefasApi.Entities;

namespace GerenciadorTarefasApi.Infra.Repositories.Interfaces;

public interface ITarefaRepository
{
    List<Tarefa> ObterTodos();
    Tarefa? ObterPorId(int id);
    Tarefa Adicionar(Tarefa novaTarefa);

    Tarefa? Atualizar(Tarefa tarefa);
     void Deletar(Tarefa tarefa);
    bool SalvarAlteracoes();

    TarefaTag AdicionarTarefaTag(TarefaTag tarefaTag);
}
