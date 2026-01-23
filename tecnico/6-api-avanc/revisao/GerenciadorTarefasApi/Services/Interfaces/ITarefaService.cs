using System;
using GerenciadorTarefasApi.Entities;
using GerenciadorTarefasApi.Infra.DTOs;

namespace GerenciadorTarefasApi.Services.Interfaces
{
    public interface ITarefaService
    {
        List<TarefaDto> ObterTodos();
        TarefaDto? ObterPorId(int id);
        TarefaDto Adicionar(CriarTarefaDto tarefaDto);

        TarefaDto? Atualizar(int id, AtualizarTarefaDto tarefaDto);
        bool Deletar(int id);
        bool MarcarComoConcluida(int id);

        bool AssociarTag(int tarefaId, int tagId);
    }
}
