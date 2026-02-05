using AutoMapper;
using ITB.Application.Dtos;
using ITB.Domain.Entities;

namespace ITB.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Consultas
        CreateMap<Fabricante, FabricanteReadDto>();

        CreateMap<Categoria, CategoriaReadDto>();

        CreateMap<Produto, ProdutoReadDto>();

        // Inserção
        CreateMap<FabricanteCreateDto, Fabricante>();

        CreateMap<CategoriaCreateDto, Categoria>();

        CreateMap<ProdutoCreateDto, Produto>();

        // Atualização
        CreateMap<FabricanteUpdateDto, Fabricante>();       

        CreateMap<CategoriaUpdateDto, Categoria>(); 

        CreateMap<ProdutoUpdateDto, Produto>();
    }
}