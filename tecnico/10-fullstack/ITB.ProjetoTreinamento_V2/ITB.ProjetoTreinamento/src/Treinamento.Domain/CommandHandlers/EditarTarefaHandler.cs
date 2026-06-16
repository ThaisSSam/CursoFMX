using System;
using System.Threading.Tasks;
using Treinamento.Domain.Commands;
using Treinamento.Domain.Aggregates.Tarefa;
using Treinamento.Domain.Aggregates.Tarefa.Interfaces;

namespace Treinamento.Domain.Handlers;

public class EditarTarefaHandler
{
    private readonly ITarefaRepository _tarefaRepository;
    private readonly ITarefaHistoricoRepository _historicoRepository;

    public EditarTarefaHandler(ITarefaRepository tarefaRepository, ITarefaHistoricoRepository historicoRepository)
    {
        _tarefaRepository = tarefaRepository;
        _historicoRepository = historicoRepository;
    }

    public async Task<bool> ExecutarAsync(EditarTarefaCommand command)
    {
        var tarefa = await _tarefaRepository.ObterPorIdAsync(command.Id);

        if (tarefa == null)
        {
            throw new Exception("Tarefa não encontrada para atualização.");
        }

        tarefa.AtualizarDados(
            command.Nome,
            (TipoSituacao)command.Situacao,
            (TipoPrioridade)command.Prioridade,
            command.UsuarioId
        );

        await _tarefaRepository.AtualizarAsync(tarefa);

        var historico = new TarefaHistorico(tarefa, "Editar");
        await _historicoRepository.AdicionarAsync(historico);

        return true;
    }
}