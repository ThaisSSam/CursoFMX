using System;
using System.Reflection.Metadata;
using ITB.Application.Dtos;

namespace ITB.Application.Interfaces;

public interface ICategoriaService
{
    Task<IEnumerable<CategoriaReadDto>>ObterTodosAsync();

    Task<CategoriaReadDto>ObterPorIdAsync(int id);

    Task<CategoriaReadDto>AdicionarAsync(CategoriaCreateDto dto);

    Task<bool> AtualizarAsync(CategoriaUpdateDto dto);

    Task<bool> RemoverAsync(int id);
}
