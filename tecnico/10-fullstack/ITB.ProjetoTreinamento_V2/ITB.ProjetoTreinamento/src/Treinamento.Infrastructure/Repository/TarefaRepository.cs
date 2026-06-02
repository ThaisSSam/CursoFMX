using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Treinamento.Domain.Aggregates.Tarefa;
using Treinamento.Domain.Aggregates.Tarefa.Interfaces;

namespace Treinamento.Infrastructure.Persistence.Repositories;

public class TarefaRepository : ITarefaRepository
{
  private readonly TreinamentoWriteContext _context;

  public TarefaRepository(TreinamentoWriteContext context)
  {
    _context = context;
  }

  public async Task AdicionarAsync(Tarefa tarefa)
  {
    await _context.Tarefa.AddAsync(tarefa);
  }

  public void Atualizar(Tarefa tarefa)
  {
    _context.Tarefa.Update(tarefa);
  }

  public void Remover(Tarefa tarefa)
  {
    _context.Tarefa.Remove(tarefa);
  }

  public async Task<Tarefa?> ObterPorIdAsync(int id)
  {
    return await _context.Tarefa
        .FirstOrDefaultAsync(t => t.Id == id);
  }

  public async Task<IEnumerable<Tarefa>> ObterTodasAsync()
  {
    return await _context.Tarefa.ToListAsync();
  }
}