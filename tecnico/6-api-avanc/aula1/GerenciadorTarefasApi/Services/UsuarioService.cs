using AutoMapper;
using GerenciadorTarefasApi.Entities;
using GerenciadorTarefasApi.Infra.DTOs;
using GerenciadorTarefasApi.Infra.Repositories.Interfaces;
using GerenciadorTarefasApi.Services.Interfaces;
using System.Collections.Generic;

namespace GerenciadorTarefasApi.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IMapper _mapper;

        public UsuarioService(IUsuarioRepository usuarioRepository, IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
        }

        public List<UsuarioDto> ObterTodos()
        {
            var usuarios = _usuarioRepository.ObterTodos();
            return _mapper.Map<List<UsuarioDto>>(usuarios);
        }

        public UsuarioDto? ObterPorId(int id)
        {
            var usuario = _usuarioRepository.ObterPorId(id, incluirTarefas: true); 
            return _mapper.Map<UsuarioDto?>(usuario);
        }

        public UsuarioDto Adicionar(CriarUsuarioDto usuarioDto)
        {
            var novoUsuario = _mapper.Map<Usuario>(usuarioDto);
            var usuarioAdicionado = _usuarioRepository.Adicionar(novoUsuario);
            return _mapper.Map<UsuarioDto>(usuarioAdicionado);
        }

        public UsuarioDto? Atualizar(int id, CriarUsuarioDto usuarioDto)
        {
            var usuarioExistente = _usuarioRepository.ObterPorId(id);

            if (usuarioExistente == null)
            {
                return null;
            }

            _mapper.Map(usuarioDto, usuarioExistente);
            var usuarioAtualizado = _usuarioRepository.Atualizar(usuarioExistente);

            return _mapper.Map<UsuarioDto?>(usuarioAtualizado);
        }

        public bool Deletar(int id)
        {
            var usuarioExistente = _usuarioRepository.ObterPorId(id);

            if (usuarioExistente == null)
            {
                return false;
            }

            _usuarioRepository.Deletar(usuarioExistente);
            return true;
        }
    }
}