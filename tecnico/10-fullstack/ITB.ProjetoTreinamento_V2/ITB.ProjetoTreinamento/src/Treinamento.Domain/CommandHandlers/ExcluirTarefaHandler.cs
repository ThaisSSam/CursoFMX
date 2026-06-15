using System;
using System.Threading.Tasks;
using Treinamento.Domain.Commands;
using Treinamento.Domain.Aggregates.Tarefa.Interfaces;

namespace Treinamento.Domain.Handlers;

public class ExcluirTarefaHandler
{
    private readonly ITarefaRepository _tarefaRepository;

    public ExcluirTarefaHandler(ITarefaRepository tarefaRepository)
    {
        _tarefaRepository = tarefaRepository;
    }

    public async Task<bool> ExecutarAsync(ExcluirTarefaCommand command)
    {
        var tarefa = await _tarefaRepository.ObterPorIdAsync(command.Id);

        if (tarefa == null)
        {
            throw new Exception("Tarefa não encontrada para exclusão.");
        }

        await _tarefaRepository.RemoverAsync(tarefa);
        return true;
    }
}