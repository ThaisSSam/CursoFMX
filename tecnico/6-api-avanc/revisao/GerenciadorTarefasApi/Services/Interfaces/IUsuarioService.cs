using GerenciadorTarefasApi.Infra.DTOs;
using System.Collections.Generic;

namespace GerenciadorTarefasApi.Services.Interfaces
{
    public interface IUsuarioService
    {
        List<UsuarioDto> ObterTodos();
        UsuarioDto? ObterPorId(int id);
        UsuarioDto Adicionar(CriarUsuarioDto usuarioDto);
        UsuarioDto? Atualizar(int id, CriarUsuarioDto usuarioDto); 
        bool Deletar(int id);
    }
}