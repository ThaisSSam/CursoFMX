using System.Collections.Generic;
using System.Threading.Tasks;

namespace Treinamento.Domain.Aggregates.Tarefa.Interfaces;

public interface ITarefaRepository
{
    Task AdicionarAsync(Tarefa tarefa);
    void Atualizar(Tarefa tarefa);
    void Remover(Tarefa tarefa);
    Task<Tarefa?> ObterPorIdAsync(int id);
    Task<IEnumerable<Tarefa>> ObterTodasAsync();
}