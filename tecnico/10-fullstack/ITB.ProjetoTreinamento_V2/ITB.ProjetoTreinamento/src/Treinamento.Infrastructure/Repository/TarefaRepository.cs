using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Treinamento.Domain.Aggregates.Tarefa;
using Treinamento.Domain.Aggregates.Tarefa.Interfaces;

namespace Treinamento.Infrastructure.Persistence.Repositories;

public class TarefaRepository : ITarefaRepository
{
  private readonly TreinamentoContext _context;

  public TarefaRepository(TreinamentoContext context)
  {
    _context = context;
  }

  public async Task AdicionarAsync(Tarefa tarefa)
  {
    await _context.Tarefa.AddAsync(tarefa);
    await _context.SaveChangesAsync();
  }

  public async Task AtualizarAsync(Tarefa tarefa)
  {
    _context.Tarefa.Update(tarefa);
    await _context.SaveChangesAsync();
  }

  public async Task RemoverAsync(Tarefa tarefa)
  {
    _context.Tarefa.Remove(tarefa);
    await _context.SaveChangesAsync();
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