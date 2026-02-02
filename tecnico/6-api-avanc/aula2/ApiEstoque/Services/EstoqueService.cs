using System;
using System.Threading.Tasks;
using ApiEstoque.Entities;
using ApiEstoque.Infra.DTOs;
using ApiEstoque.Infra.Repositories.Interfaces;
using ApiEstoque.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ApiEstoque.Services;

public class EstoqueService : IEstoqueService
{

    private readonly IProdutoDBRepository _produtoDBRepository;
    private readonly IMapper _mapper;

    public EstoqueService(IProdutoDBRepository produtoDBRepository, IMapper mapper)
    {
        _produtoDBRepository = produtoDBRepository;
        _mapper = mapper;
    }

    public async Task<List<ProdutoDto>> ObterTodosAsync()
    {
        var produtos = await _produtoDBRepository.ObterTodosAsync();
        var produtosList = produtos.ToList();

        return _mapper.Map<List<ProdutoDto>>(produtosList);
    }

    public async Task<ProdutoDto?> ObterPorIdAsync(int id)
    {
        var produto = await _produtoDBRepository.ObterPorIdAsync(id);
        return _mapper.Map<ProdutoDto>(produto);
    }

    public async Task<FabricanteDto?> ObterFabricantePorIdAsync(int id)
    {
        var fabricante = await _produtoDBRepository.ObterPorIdAsync(id);
        return _mapper.Map<FabricanteDto>(fabricante);
    }

    public async Task<ProdutoDto> AdicionarAsync(CriarProdutoDto produtoDto)
    {
        var novoProduto = _mapper.Map<Produto>(produtoDto);
        var produtoAdicionado = await _produtoDBRepository.AdicionarAsync(novoProduto);
        return _mapper.Map<ProdutoDto>(produtoAdicionado);
    }

    public async Task<bool> DeletarAsync(int id)
    {
        var produtoExistente = await _produtoDBRepository.ObterPorIdAsync(id);
        if (produtoExistente == null)
        {
            return false;
        }

        await _produtoDBRepository.DeletarAsync(produtoExistente);
        return true;
    }

    Task IEstoqueService.DeletarAsync(int id)
    {
        return DeletarAsync(id);
    }

    Task IEstoqueService.ObterFabricantePorIdAsync(int id)
    {
        return ObterFabricantePorIdAsync(id);
    }

    public async Task<ProdutoDto?> AtualizarAsync(int id, CriarProdutoDto produtoDto)
{
    var produtoExistente = await _produtoDBRepository.ObterPorIdAsync(id);

    if (produtoExistente == null)
    {
        return null;
    }

    _mapper.Map(produtoDto, produtoExistente);

    await _produtoDBRepository.AtualizarAsync(produtoExistente);

    var produtoAtualizado = await _produtoDBRepository.ObterPorIdAsync(id);

    return _mapper.Map<ProdutoDto>(produtoAtualizado);
}
}
