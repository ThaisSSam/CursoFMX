using System;
using System.Reflection.Emit;
using AutoMapper;
using ITB.Application.Dtos;
using ITB.Application.Interfaces;
using ITB.Domain.Entities;
using ITB.Domain.Interfaces;

namespace ITB.Application.Services;

public class CategoriaService : ICategoriaService
{
    private readonly ICategoriaRepository   _repository;

    private readonly IMapper _mapper;

    public CategoriaService(ICategoriaRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CategoriaReadDto>> ObterTodosAsync()
    {
        var categorias = await _repository.ObterTodosAsync();
        return _mapper.Map<IEnumerable<CategoriaReadDto>>(categorias);
    }

    public async Task<CategoriaReadDto?> ObterPorIdAsync(int id)
    {
        var categoria = await _repository.ObterPorIdAsync(id);
        return _mapper.Map<CategoriaReadDto>(categoria);
    }

    public async Task<CategoriaReadDto>AdicionarAsync(CategoriaCreateDto dto)
    {
        var categoria= _mapper.Map<Categoria>(dto);
        await _repository.AdicionarAsync(categoria);
        return _mapper.Map<CategoriaReadDto>(categoria);
    }

    public async Task<bool> AtualizarAsync(CategoriaUpdateDto dto)
    {
        var categoriaBanco = await _repository.ObterPorIdAsync(dto.Id);

        if (categoriaBanco == null) return false;

        // 2. O AutoMapper copia as propriedades do DTO para a Entidade do banco
        _mapper.Map(dto, categoriaBanco);

        // 3. O repositório salva a entidade que já está com os valores novos
        await _repository.AtualizarAsync(categoriaBanco);
        return true;
    }

    public async Task<bool> RemoverAsync(int id)
    {
        return await _repository.RemoverAsync(id);
    }
}
