using System.Collections.Generic;
using System.Threading.Tasks;

namespace Treinamento.Domain.Aggregates.Tarefa.Interfaces;

public interface ITarefaRepository
{
    Task AdicionarAsync(Tarefa tarefa);
    Task AtualizarAsync(Tarefa tarefa);
    Task RemoverAsync(Tarefa tarefa);
    Task<Tarefa?> ObterPorIdAsync(int id);
    Task<IEnumerable<Tarefa>> ObterTodasAsync();
}