using System;
using System.Threading.Tasks;
using Treinamento.Domain.Commands;
using Treinamento.Domain.Aggregates.Tarefa;
using Treinamento.Domain.Aggregates.Tarefa.Interfaces;

namespace Treinamento.Domain.Handlers;

public class ExcluirTarefaHandler
{
    private readonly ITarefaRepository _tarefaRepository;
    private readonly ITarefaHistoricoRepository _historicoRepository; 

    public ExcluirTarefaHandler(ITarefaRepository tarefaRepository, ITarefaHistoricoRepository historicoRepository)
    {
        _tarefaRepository = tarefaRepository;
        _historicoRepository = historicoRepository;
    }

    public async Task<bool> ExecutarAsync(ExcluirTarefaCommand command)
    {
        var tarefa = await _tarefaRepository.ObterPorIdAsync(command.Id);

        if (tarefa == null)
        {
            throw new Exception("Tarefa não encontrada para exclusão.");
        }

        tarefa.MarcarComoExcluido();

        await _tarefaRepository.AtualizarAsync(tarefa);

        var historico = new TarefaHistorico(tarefa, "Excluir");
        await _historicoRepository.AdicionarAsync(historico);

        return true;
    }
}