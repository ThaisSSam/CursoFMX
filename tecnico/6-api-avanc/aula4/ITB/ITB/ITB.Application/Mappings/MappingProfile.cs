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

        // Inserção
        CreateMap<FabricanteCreateDto, Fabricante>();

        // Atualização
        CreateMap<FabricanteUpdateDto, Fabricante>();

        CreateMap<Categoria, CategoriaReadDto>();

        CreateMap<CategoriaCreateDto, Categoria>();

        CreateMap<CategoriaUpdateDto, Categoria>();
    }
}