using System;
using AutoMapper;
using ITB.Application.Dtos;
using ITB.Application.Interfaces;
using ITB.Domain.Entities;
using ITB.Domain.Interfaces;

namespace ITB.Application.Services;

public class ProdutoService : IProdutoService {
    private readonly IProdutoRepository _repository;
    private readonly IMapper _mapper;

    public ProdutoService(IProdutoRepository repository, IMapper mapper) {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProdutoReadDto>> ObterTodosAsync() {
        var produtos = await _repository.ObterTodosAsync();
        return _mapper.Map<IEnumerable<ProdutoReadDto>>(produtos);
    }

    public async Task<ProdutoReadDto?> ObterPorIdAsync(int id) {
        var produto = await _repository.ObterPorIdAsync(id);
        return _mapper.Map<ProdutoReadDto>(produto);
    }

    public async Task<ProdutoReadDto> AdicionarAsync(ProdutoCreateDto dto) {
        var produto = _mapper.Map<Produto>(dto);
    
        var produtoAdicionado = await _repository.AdicionarAsync(produto);

        var produtoCompleto = await _repository.ObterPorIdAsync(produtoAdicionado.id);

        return _mapper.Map<ProdutoReadDto>(produtoCompleto);
    }

    public async Task<bool> AtualizarAsync(int id, ProdutoUpdateDto dto) {
        var existente = await _repository.ObterPorIdAsync(id);
        if (existente == null) return false;
        
        _mapper.Map(dto, existente);
        return await _repository.AtualizarAsync(existente);
    }

    public async Task<bool> DeletarAsync(int id) {
        var existente = await _repository.ObterPorIdAsync(id);
        if (existente == null) return false;
        return await _repository.DeletarAsync(existente);
    }
}