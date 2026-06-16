using System;
using System.Threading.Tasks;
using Treinamento.Domain.Aggregates.Tarefa;
using Treinamento.Domain.Aggregates.Tarefa.Interfaces;
using Treinamento.Domain.Aggregates.Usuarios;
using Treinamento.Domain.Commands;

namespace Treinamento.Domain.Handlers;

public class CriarTarefaHandler
{
    private readonly ITarefaRepository _tarefaRepository;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly ITarefaHistoricoRepository _historicoRepository; 

    public CriarTarefaHandler(
        ITarefaRepository tarefaRepository, 
        IUsuarioRepository usuarioRepository,
        ITarefaHistoricoRepository historicoRepository)
    {
        _tarefaRepository = tarefaRepository;
        _usuarioRepository = usuarioRepository;
        _historicoRepository = historicoRepository;
    }

    public async Task<bool> ExecutarAsync(CriarTarefaCommand request)
    {
        if (string.IsNullOrWhiteSpace(request.Nome))
            throw new Exception("O título da tarefa não pode estar vazio.");

        var usuario = await _usuarioRepository.ObterPorIdAsync(request.UsuarioId);
        if (usuario == null)
            throw new Exception("O usuário responsável informado não existe.");

        var novaTarefa = new Tarefa(
            request.Nome,
            request.UsuarioId,
            (TipoSituacao)request.Situacao,
            (TipoPrioridade)request.Prioridade
        );

        await _tarefaRepository.AdicionarAsync(novaTarefa);

        var historico = new TarefaHistorico(novaTarefa, "Criar");
        await _historicoRepository.AdicionarAsync(historico);

        return true;
    }
}