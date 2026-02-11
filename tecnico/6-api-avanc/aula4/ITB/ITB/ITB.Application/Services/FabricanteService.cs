using AutoMapper;
using ITB.Application.Dtos;
using ITB.Application.Interfaces;
using ITB.Domain.Entities;
using ITB.Domain.Interfaces;

namespace ITB.Application.Services;

public class FabricanteService : IFabricanteService
{
    private readonly IFabricanteRepository _repository;
    private readonly IProdutoRepository _produtoRepository;
    private readonly IMapper _mapper;

    public FabricanteService(IFabricanteRepository repository,IProdutoRepository produtoRepository, IMapper mapper)
    {
        _repository = repository;
        _produtoRepository = produtoRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<FabricanteReadDto>> ObterTodosAsync()
    {
        var fabricantes = await _repository.ObterTodosAsync();
        return _mapper.Map<IEnumerable<FabricanteReadDto>>(fabricantes);
    }

    public async Task<FabricanteReadDto?> ObterPorIdAsync(int id)
    {
        var fabricante = await _repository.ObterPorIdAsync(id);
        return _mapper.Map<FabricanteReadDto>(fabricante);
    }

    public async Task<FabricanteReadDto> AdicionarAsync(FabricanteCreateDto dto)
    {
        var fabricante = _mapper.Map<Fabricante>(dto);
        await _repository.AdicionarAsync(fabricante);
        return _mapper.Map<FabricanteReadDto>(fabricante);
    }

    public async Task<bool> AtualizarAsync(FabricanteUpdateDto dto)
    {
        // 1. Busca a entidade original rastreada pelo EF
        var fabricanteBanco = await _repository.ObterPorIdAsync(dto.Id);

        if (fabricanteBanco == null) return false;

        // 2. O AutoMapper copia as propriedades do DTO para a Entidade do banco
        _mapper.Map(dto, fabricanteBanco);

        // 3. O repositório salva a entidade que já está com os valores novos
        await _repository.AtualizarAsync(fabricanteBanco);
        return true;
    }

    public async Task<bool> RemoverAsync(int id)
    {
        var temProdutos = await _produtoRepository.QualquerProdutoComFabricante(id);
        
        if (temProdutos)
        {
            throw new Exception("Não é possível excluir um fabricante que possui produtos cadastrados.");
        }

        return await _repository.RemoverAsync(id);
    }
}