using System;
using AutoMapper;
using LojaApi.Entities;
using LojaApi.Infra.DTOs;

namespace LojaApi.Infra.Mapping;


/// Define o perfil de mapeamento para o AutoMapper.
/// O AutoMapper procura automaticamente por classes que herdam de 'Profile'
/// na inicialização da aplicação (geralmente no Program.cs ou Startup.cs)
/// para carregar essas configurações.
public class MappingProfile : Profile
{
    /// O construtor é onde todas as regras de mapeamento são definidas
    /// usando o método CreateMap<Origem, Destino>().
    public MappingProfile()
    {       

        // Mapeia a entidade de junção PedidoProduto para o DTO do item (ProdutoPedidoDto)
        // Este é um exemplo clássico de "Flattening" (Achatamento).
        // Estamos transformando um objeto complexo (com relações [tem produto? na sua entidade]) em um DTO "plano".
        CreateMap<PedidoProduto, ProdutoPedidoDto>()
            // Mapeia PedidoProduto.ProdutoId para ProdutoPedidoDto.ProdutoId
            .ForMember(dest => dest.ProdutoId, opt => opt.MapFrom(src => src.ProdutoId))

            // 'dest.NomeProduto' (no DTO) não existe diretamente em PedidoProduto.
            // Precisamos "navegar" pela propriedade 'Produto' (se ela foi incluída na consulta!)
            // e pegar o 'Nome'.
            // A verificação 'src.Produto != null' é uma boa prática de programação defensiva
            // para evitar NullReferenceException.
            .ForMember(dest => dest.NomeProduto, opt => opt.MapFrom(src => src.Produto != null ? src.Produto.Nome : string.Empty))

            // Mesma lógica de "Flattening" para o Preço.
            // Se o produto for nulo, retornamos 0m (decimal zero).
            .ForMember(dest => dest.PrecoUnitario, opt => opt.MapFrom(src => src.Produto != null ? src.Produto.Preco : 0m))

            // Mapeamento direto (convenção).
            .ForMember(dest => dest.Quantidade, opt => opt.MapFrom(src => src.Quantidade));

        // Mapeia a entidade Pedido para o DTO detalhado (PedidoDetalhadoDto)
        CreateMap<Pedido, PedidoDetalhadoDto>()
            // "Flattening" para buscar o nome do cliente a partir da relação.
            .ForMember(dest => dest.NomeCliente, opt => opt.MapFrom(src => src.Cliente != null ? src.Cliente.Nome : string.Empty))

            // Mapeamento da coleção de itens.
            // 'dest.Itens' (no DTO) é provavelmente uma List<ProdutoPedidoDto>
            // 'src.PedidoProdutos' (na Entidade) é uma List<PedidoProduto>
            //
            // A "mágica" do AutoMapper: Ele vê que precisa mapear uma coleção.
            // Ele então procura um mapa que saiba converter *um item* da lista de origem
            // (PedidoProduto) para *um item* da lista de destino (ProdutoPedidoDto).
            // Como definimos esse mapa exatamente acima (CreateMap<PedidoProduto, ProdutoPedidoDto>),
            // ele o aplicará automaticamente para cada item da coleção.
            .ForMember(dest => dest.Itens, opt => opt.MapFrom(src => src.PedidoProdutos));

        // Mapeamento de Produto (Entidade) para ProdutoResumoDto (DTO)
        // Também usa mapeamento por convenção.
        CreateMap<Produto, ProdutoResumoDto>();


        CreateMap<Cliente, ClienteDetalhadoDto>();
        CreateMap<Cliente, CriarClienteDto>()
            .ForMember(dest => dest.Endereco, opt => opt.MapFrom(src => src.Endereco)).ReverseMap();


    }
}
