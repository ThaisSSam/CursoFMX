using System;
using System.Threading.Tasks;
using ApiEstoque.Entities;
using ApiEstoque.Infra.Context;
using ApiEstoque.Infra.DTOs;
using ApiEstoque.Infra.Repositories.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ApiEstoque.Infra.Repositories;

public class ProdutoDBRepository : IProdutoDBRepository
{
    private readonly LojaDbContext _context;
    private readonly IMapper _mapper;

    public ProdutoDBRepository(LojaDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<Produto>> ObterTodosAsync()
    {
        return await _context.Produtos.Include(p => p.Fabricante).ToListAsync();
    }

    // public Produto? ObterPorId(int id)
    // {
    //     return _context.Produtos
    //         .Include(p => p.Fabricante)
    //         .FirstOrDefault(p => p.Id == id);
    // }
    public async Task <Produto?> ObterPorIdAsync(int id)
    {
        return await _context.Produtos
            .Include(p => p.Fabricante)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Produto> AdicionarAsync(Produto novoProduto)
    {
        // 1. Adiciona diretamente ao DbSet do contexto
        await _context.Produtos.AddAsync(novoProduto);
        await _context.SaveChangesAsync();

        // 2. Carrega explicitamente a referência do fabricante para que ela não seja null no retorno
        await _context.Entry(novoProduto).Reference(p => p.Fabricante).LoadAsync();

        return novoProduto;
    }

    public async Task<Produto> AtualizarAsync(Produto produtoModificado)
    {
        var produtoAlterado = _mapper.Map<Produto>(produtoModificado);
        _context.Produtos.Update(produtoAlterado);
        await _context.SaveChangesAsync();
        return produtoAlterado;
    }

    public async Task DeletarAsync(Produto produto)
    {
        _context.Produtos.Remove(produto);
        await _context.SaveChangesAsync();
    }


    async Task<Produto?> IProdutoDBRepository.ObterFabricantePorIdAsync(int FabricanteId)
    {
        return await _context.Produtos.FirstOrDefaultAsync(f => f.FabricanteId == FabricanteId);
    }
}
