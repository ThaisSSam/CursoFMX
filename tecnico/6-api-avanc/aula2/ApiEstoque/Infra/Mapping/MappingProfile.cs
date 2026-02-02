using System;
using ApiEstoque.Entities;
using ApiEstoque.Infra.DTOs;
using AutoMapper;

namespace ApiEstoque.Services;

public class MappingProfile : Profile
{

    public MappingProfile()
    {
        /*
        CreateMap<Produto, ProdutoDto>()
            .ReverseMap();
        */
        CreateMap<Fabricante, FabricanteDto>()
            .ReverseMap();

        CreateMap<Produto, ProdutoDto>()
            .ForMember(dest => dest.Id, opt=> opt.MapFrom(src=> src.Id))
            .ForMember(dest => dest.Nome, opt=> opt.MapFrom(src=> src.Nome))
            .ForMember(dest => dest.Preco, opt=> opt.MapFrom(src=> src.Preco))
            .ForMember(dest => dest.FabricanteId, opt=> opt.MapFrom(src=> src.FabricanteId))
            .ForMember(dest => dest.FabricanteDetalhe, opt=> opt.MapFrom(src => src.Fabricante))
            ;
        
        CreateMap<CriarProdutoDto, Produto>();      

        CreateMap<CriarFabricanteDto, Fabricante>();


    }
}
