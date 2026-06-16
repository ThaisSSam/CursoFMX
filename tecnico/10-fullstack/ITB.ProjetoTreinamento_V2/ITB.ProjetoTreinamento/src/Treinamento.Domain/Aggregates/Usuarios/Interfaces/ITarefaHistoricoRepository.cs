using System.Threading.Tasks;

namespace Treinamento.Domain.Aggregates.Tarefa.Interfaces;

public interface ITarefaHistoricoRepository
{
    Task AdicionarAsync(TarefaHistorico historico);
}