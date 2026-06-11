using Treinamento.Domain.Aggregates.Tarefa;
using Treinamento.Domain.Aggregates.Tarefa.Interfaces;
using Treinamento.Domain.Aggregates.Usuarios;
using Treinamento.Domain.Commands;

namespace Treinamento.Domain.Handlers;

public class CriarTarefaHandler
{
    private readonly ITarefaRepository _tarefaRepository;
    private readonly IUsuarioRepository _usuarioRepository;

    public CriarTarefaHandler(ITarefaRepository tarefaRepository, IUsuarioRepository usuarioRepository)
    {
        _tarefaRepository = tarefaRepository;
        _usuarioRepository = usuarioRepository;
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

        return true;
    }
}