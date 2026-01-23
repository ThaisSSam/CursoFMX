using AutoMapper;
using GerenciadorTarefasApi.Entities;
using GerenciadorTarefasApi.Infra.DTOs;
using GerenciadorTarefasApi.Infra.Repositories.Interfaces;
using GerenciadorTarefasApi.Services.Interfaces;
using System.Collections.Generic;

namespace GerenciadorTarefasApi.Services
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _tagRepository;
        private readonly IMapper _mapper;

        public TagService(ITagRepository tagRepository, IMapper mapper)
        {
            _tagRepository = tagRepository;
            _mapper = mapper;
        }

        public List<TagDto> ObterTodos()
        {
            var tags = _tagRepository.ObterTodos();
            return _mapper.Map<List<TagDto>>(tags);
        }

        public TagDto? ObterPorId(int id)
        {
            var tag = _tagRepository.ObterPorId(id);
            return _mapper.Map<TagDto?>(tag);
        }

        public TagDto Adicionar(CriarTagDto tagDto)
        {
            var novaTag = _mapper.Map<Tag>(tagDto);
            var tagAdicionada = _tagRepository.Adicionar(novaTag);
            return _mapper.Map<TagDto>(tagAdicionada);
        }

        public TagDto? Atualizar(int id, CriarTagDto tagDto)
        {
            var tagExistente = _tagRepository.ObterPorId(id);

            if (tagExistente == null)
            {
                return null;
            }

            _mapper.Map(tagDto, tagExistente);
            var tagAtualizada = _tagRepository.Atualizar(tagExistente);

            return _mapper.Map<TagDto?>(tagAtualizada);
        }

        public bool Deletar(int id)
        {
            var tagExistente = _tagRepository.ObterPorId(id);

            if (tagExistente == null)
            {
                return false;
            }

            _tagRepository.Deletar(tagExistente);
            return true;
        }
    }
}