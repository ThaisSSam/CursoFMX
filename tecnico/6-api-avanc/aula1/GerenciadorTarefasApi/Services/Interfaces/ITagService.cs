using GerenciadorTarefasApi.Infra.DTOs;
using System.Collections.Generic;

namespace GerenciadorTarefasApi.Services.Interfaces
{
    public interface ITagService
    {
        List<TagDto> ObterTodos();
        TagDto? ObterPorId(int id);
        TagDto Adicionar(CriarTagDto tagDto);
        TagDto? Atualizar(int id, CriarTagDto tagDto);
        bool Deletar(int id);
    }
}