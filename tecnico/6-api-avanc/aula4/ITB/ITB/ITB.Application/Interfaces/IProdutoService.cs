using System;
using ITB.Application.Dtos;

namespace ITB.Application.Interfaces;

public interface IProdutoService {
    Task<IEnumerable<ProdutoReadDto>> ObterTodosAsync();
    Task<ProdutoReadDto?> ObterPorIdAsync(int id);
    Task<ProdutoReadDto> AdicionarAsync(ProdutoCreateDto dto);
    Task<bool> AtualizarAsync(int id, ProdutoUpdateDto dto);
    Task<bool> DeletarAsync(int id);
}