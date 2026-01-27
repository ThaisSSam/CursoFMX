using System;
using ApiEstoque.Entities;
using ApiEstoque.Infra.Context;
using ApiEstoque.Infra.DTOs;
using ApiEstoque.Infra.Repositories.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ApiEstoque.Infra.Repositories;

public class FabricanteDBRepository : IFabricanteDBRepository
{
    private readonly LojaDbContext _context;
    private readonly IMapper _mapper;

    public FabricanteDBRepository(LojaDbContext context, IMapper mapper)
    {
        _context = context; 
        _mapper = mapper;
    }

    public async Task<List<Fabricante>> ObterTodosFabricanteAsync()
    {
        return await _context.Fabricantes.ToListAsync();
    }


    public async Task<Fabricante?> ObterFabricantePorIdAsync(int id)
    {
        return await _context.Fabricantes.FirstOrDefaultAsync(f => f.Id == id); 
    }

    public async Task<Fabricante> AdicionarFabricanteAsync(Fabricante novoFabricante)
    {
        await _context.Fabricantes.AddAsync(novoFabricante);
        await _context.SaveChangesAsync();

        return novoFabricante;
    }

    public async Task<Fabricante> AtualizarFabricanteAsync(FabricanteDto fabricanteModificado)
    {
        var fabricanteAlterado = _mapper.Map<Fabricante>(fabricanteModificado);
        _context.Fabricantes.Update(fabricanteAlterado);
        await _context.SaveChangesAsync();
        return fabricanteAlterado;
    }

    public async Task DeletarFabricanteAsync(Fabricante fabricante)
    {
        _context.Fabricantes.Remove(fabricante);
        await _context.SaveChangesAsync();
    }

    Task<Fabricante> IFabricanteDBRepository.AdicionarFabricanteAsync(Fabricante novoFabricante)
    {
        return AdicionarFabricanteAsync(novoFabricante);
    }

    Task<List<Fabricante>> IFabricanteDBRepository.ObterTodosFabricanteAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Fabricante> AtualizarFabricanteAsync(Fabricante fabricante)
    {
        throw new NotImplementedException();
    }
}
