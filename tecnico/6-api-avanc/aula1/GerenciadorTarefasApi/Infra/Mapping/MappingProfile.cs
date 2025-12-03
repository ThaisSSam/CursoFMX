using AutoMapper;
using GerenciadorTarefasApi.Entities;
using GerenciadorTarefasApi.Infra.DTOs;

namespace GerenciadorTarefasApi.Infra.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CriarUsuarioDto, Usuario>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Tarefas, opt => opt.Ignore());

            CreateMap<Usuario, UsuarioDto>();
            CreateMap<Tarefa, TarefaDtoSimples>();


            CreateMap<CriarTarefaDto, Tarefa>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Concluida, opt => opt.Ignore())
                .ForMember(dest => dest.DataCriacao, opt => opt.Ignore())
                .ForMember(dest => dest.DataConclusao, opt => opt.Ignore())
                .ForMember(dest => dest.Usuario, opt => opt.Ignore())
                .ForMember(dest => dest.Detalhes, opt => opt.Ignore())
                .ForMember(dest => dest.TarefasTags, opt => opt.Ignore());

                CreateMap<AtualizarTarefaDto, Tarefa>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.DataCriacao, opt => opt.Ignore())
                .ForMember(dest => dest.UsuarioId, opt => opt.Ignore())
                .ForMember(dest => dest.Usuario, opt => opt.Ignore())
                .ForMember(dest => dest.Detalhes, opt => opt.Ignore())
                .ForMember(dest => dest.TarefasTags, opt => opt.Ignore());
                
            CreateMap<Tarefa, TarefaDto>();
            CreateMap<Usuario, UsuarioResumoDto>();
            CreateMap<DetalhesTarefa, DetalhesTarefaDto>();

            CreateMap<CriarTagDto, Tag>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.TarefasTags, opt => opt.Ignore());

            // NOVO: Mapeamento de Saídas (GET Tag)
            CreateMap<Tag, TagDto>();
        }
    }
}