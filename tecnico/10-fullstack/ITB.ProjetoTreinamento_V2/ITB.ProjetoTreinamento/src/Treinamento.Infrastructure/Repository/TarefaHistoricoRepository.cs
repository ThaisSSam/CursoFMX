using System.Threading.Tasks;
using Treinamento.Domain.Aggregates.Tarefa;
using Treinamento.Domain.Aggregates.Tarefa.Interfaces;
using Treinamento.Infrastructure.Persistence;

namespace Treinamento.Infrastructure.Repositories;

public class TarefaHistoricoRepository : ITarefaHistoricoRepository
{
    private readonly TreinamentoReadContext _context;

    public TarefaHistoricoRepository(TreinamentoReadContext context)
    {
        _context = context;
    }

    public async Task AdicionarAsync(TarefaHistorico historico)
    {
        await _context.Set<TarefaHistorico>().AddAsync(historico);
        await _context.SaveChangesAsync();
    }
}