using System;
using System.Threading.Tasks;
using Treinamento.Domain.Commands;
using Treinamento.Domain.Aggregates.Tarefa.Interfaces;

namespace Treinamento.Domain.Handlers;

public class EditarTarefaHandler
{
    private readonly ITarefaRepository _tarefaRepository;

    public EditarTarefaHandler(ITarefaRepository tarefaRepository)
    {
        _tarefaRepository = tarefaRepository;
    }

    public async Task<bool> ExecutarAsync(EditarTarefaCommand command)
    {
        var tarefa = await _tarefaRepository.ObterPorIdAsync(command.Id);

        if (tarefa == null)
        {
            throw new Exception("Tarefa não encontrada para atualização.");
        }

        var tipoTarefa = typeof(Treinamento.Domain.Aggregates.Tarefa.Tarefa);

        tipoTarefa.GetProperty("Nome")?.SetValue(tarefa, command.Nome);
        tipoTarefa.GetProperty("UsuarioId")?.SetValue(tarefa, command.UsuarioId);

        var propriedadeSituacao = tipoTarefa.GetProperty("Situacao");
        if (propriedadeSituacao != null)
        {
            var enumSituacao = Enum.ToObject(propriedadeSituacao.PropertyType, command.Situacao);
            propriedadeSituacao.SetValue(tarefa, enumSituacao);
        }

        var propriedadeprioridade = tipoTarefa.GetProperty("Prioridade");
        if (propriedadeprioridade != null)
        {
            var enumPrioridade = Enum.ToObject(propriedadeprioridade.PropertyType, command.Prioridade);
            propriedadeprioridade.SetValue(tarefa, enumPrioridade);
        }

        await _tarefaRepository.AtualizarAsync(tarefa);

        return true;
    }
}