using System;
using System.Threading.Tasks;
using ApiEstoque.Entities;
using ApiEstoque.Infra.DTOs;
using ApiEstoque.Infra.Repositories.Interfaces;
using ApiEstoque.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ApiEstoque.Services;

public class FabricanteService : IFabricanteService
{

    private readonly IFabricanteDBRepository _fabricanteDBRepository;
    private readonly IMapper _mapper;

    public FabricanteService(IFabricanteDBRepository fabricanteDBRepository, IMapper mapper)
    {
        _fabricanteDBRepository = fabricanteDBRepository;
        _mapper = mapper;
    }

    public async Task<List<FabricanteDto>> ObterTodosFabricanteAsync()
    {
        var fabricantes = await _fabricanteDBRepository.ObterTodosFabricanteAsync();
        var fabricantesList = fabricantes.ToList();

        return _mapper.Map<List<FabricanteDto>>(fabricantesList);
    }

    public async Task<FabricanteDto?> ObterFabricantePorIdAsync(int id)
    {
        var fabricante = await _fabricanteDBRepository.ObterFabricantePorIdAsync(id);
        return _mapper.Map<FabricanteDto>(fabricante);
    }

    public async Task<FabricanteDto> AdicionarFabricanteAsync(CriarFabricanteDto fabricanteDto)
    {
        var novoFabricante = _mapper.Map<Fabricante>(fabricanteDto);
        var fabricanteAdicionado = await _fabricanteDBRepository.AdicionarFabricanteAsync(novoFabricante);
        return _mapper.Map<FabricanteDto>(fabricanteAdicionado);
    }

    public async Task<FabricanteDto> DeletarFabricanteAsync(int id)
    {
        var fabricanteExistente = await _fabricanteDBRepository.ObterFabricantePorIdAsync(id);
        if (fabricanteExistente == null)
        {
            return null;
        }

        var fabricanteDto = _mapper.Map<FabricanteDto>(fabricanteExistente);

    
        await _fabricanteDBRepository.DeletarFabricanteAsync(fabricanteExistente);
        
        return fabricanteDto;
    }

    public async Task<FabricanteDto?> AtualizarFabricanteAsync(int id, CriarFabricanteDto fabricanteDto)
    {
        var fabricanteExistente = await _fabricanteDBRepository.ObterFabricantePorIdAsync(id);

        if (fabricanteExistente == null)
        {
            return null; 
        }

        _mapper.Map(fabricanteDto, fabricanteExistente);

        await _fabricanteDBRepository.AtualizarFabricanteAsync(fabricanteExistente);

        return _mapper.Map<FabricanteDto>(fabricanteExistente);
    }

}
