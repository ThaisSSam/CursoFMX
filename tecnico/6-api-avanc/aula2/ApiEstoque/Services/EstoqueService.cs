using System;
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

    public List<ProdutoDto> ObterTodos()
    {
        var produtos = _produtoDBRepository.ObterTodos().ToList();

        return _mapper.Map<List<ProdutoDto>>(produtos);
    }

    public ProdutoDto? ObterPorId(int id)
    {
        var produto = _produtoDBRepository.ObterPorId(id);
        return _mapper.Map<ProdutoDto>(produto);
    }

    public ProdutoDto Adicionar(CriarProdutoDto produtoDto)
    {
        var novoProduto = _mapper.Map<Produto>(produtoDto);
        var produtoAdicionado = _produtoDBRepository.Adicionar(novoProduto);
        return _mapper.Map<ProdutoDto>(produtoAdicionado);
    }

    public bool Deletar(int id)
    {
        var produtoExistente = _produtoDBRepository.ObterPorId(id);
        if (produtoExistente == null)
        {
            return false;
        }

        _produtoDBRepository.Deletar(produtoExistente);
        return true;
    }

}
