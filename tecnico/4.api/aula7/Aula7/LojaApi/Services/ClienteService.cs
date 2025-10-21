// Services/ClienteService.cs
using AutoMapper;
using LojaApi.Entities;
using LojaApi.Infra.DTOs;
using LojaApi.Infra.Repositories.Interfaces;
using LojaApi.Services.Interfaces;

namespace LojaApi.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;

        private readonly IEnderecoRepository _enderecoRepository;

        private readonly IMapper _mapper;

        // O Service agora recebe sua dependência (o contrato do repositório) via construtor.
        public ClienteService(IClienteRepository clienteRepository, IEnderecoRepository enderecoRepository, IMapper mapper)
        {
            _clienteRepository = clienteRepository;
            _enderecoRepository = enderecoRepository;
            _mapper = mapper;
        }

        // Os métodos agora usam a dependência injetada (_clienteRepository)
        public List<Cliente> ObterTodos()
        {
            // Regra: Não exibir clientes inativos.
            return _clienteRepository.ObterTodos().Where(c => c.Ativo).ToList();
        }

        public Cliente? ObterPorId(int id)
        {
            return _clienteRepository.ObterPorId(id);
        }

        public CriarClienteDto Adicionar(CriarClienteDto clienteDto)
        {
            var cliente = _mapper.Map<Cliente>(clienteDto);
            // var cliente = _clienteRepository.ObterPorId(EnderecoDto.ClienteId);
            // {
            //     Nome = clienteDto.Nome.ToUpper(),
            //     Email = clienteDto.Email,
            //     Ativo = true,
            //     DataCadastro = DateTime.UtcNow,
            //     Endereco = clienteDto.Endereco != null ? new Endereco
            //     {
            //         Rua = clienteDto.Endereco.Rua,
            //         Cidade = clienteDto.Endereco.Cidade,
            //         Estado = clienteDto.Endereco.Estado,
            //         Cep = clienteDto.Endereco.Cep
            //     } : null
            // };
            var clienteNovo = _clienteRepository.Adicionar(cliente);
            return _mapper.Map<CriarClienteDto>(cliente);
        }

        public ClienteDetalhadoDto? ObterDetalhesPorId(int id)
        {
            var cliente = _clienteRepository.ObterPorId(id);
            if (cliente == null) return null;

            return new ClienteDetalhadoDto
            {
                Id = cliente.Id,
                Nome = cliente.Nome,
                Email = cliente.Email,
                Ativo = cliente.Ativo,
                Endereco = cliente.Endereco != null ? new EnderecoDto
                {
                    Rua = cliente.Endereco.Rua,
                    Cidade = cliente.Endereco.Cidade,
                    Estado = cliente.Endereco.Estado,
                    Cep = cliente.Endereco.Cep
                } : null
            };
        }

        public Cliente? Atualizar(int id, Cliente clienteAtualizado)
        {
            if (id != clienteAtualizado.Id) return null;
            return _clienteRepository.Atualizar(id, clienteAtualizado);
        }

        public bool Remover(int id)
        {
            var cliente = _clienteRepository.ObterPorId(id);
            if (cliente != null)
            {
                cliente.Ativo = false;
                return _clienteRepository.Atualizar(id, cliente) != null;
            }
            return false;
        }
    }
}