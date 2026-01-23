using AutoMapper;
using GerenciadorTarefasApi.Entities;
using GerenciadorTarefasApi.Infra.DTOs;
using GerenciadorTarefasApi.Infra.Repositories.Interfaces;
using GerenciadorTarefasApi.Services.Interfaces;


namespace GerenciadorTarefasApi.Services
{
    public class TarefaService : ITarefaService
    {
        private readonly ITarefaRepository _tarefaRepository;
        private readonly IMapper _mapper;
        private readonly ITagRepository _tagRepository;

        public TarefaService(ITarefaRepository tarefaRepository, ITagRepository tagRepository, IMapper mapper)
        {
            _tarefaRepository = tarefaRepository;
            _tagRepository = tagRepository;
            _mapper = mapper;
        }

        public List<TarefaDto> ObterTodos()
        {
            var tarefas = _tarefaRepository.ObterTodos();
            return _mapper.Map<List<TarefaDto>>(tarefas);
        }

        public TarefaDto? ObterPorId(int id)
        {
            var tarefa = _tarefaRepository.ObterPorId(id);
            return _mapper.Map<TarefaDto?>(tarefa);
        }

        public TarefaDto Adicionar(CriarTarefaDto tarefaDto)
        {
            var novaTarefa = _mapper.Map<Tarefa>(tarefaDto);
            novaTarefa.DataCriacao = DateTimeOffset.UtcNow;
            novaTarefa.Concluida = false;
            var tarefaAdicionada = _tarefaRepository.Adicionar(novaTarefa);

            return _mapper.Map<TarefaDto>(tarefaAdicionada);
        }

        public TarefaDto? Atualizar(int id, AtualizarTarefaDto tarefaDto)
        {
            var tarefaExistente = _tarefaRepository.ObterPorId(id);

            if (tarefaExistente == null)
            {
                return null;
            }


            _mapper.Map(tarefaDto, tarefaExistente);

            if (tarefaDto.UsuarioId.HasValue)
            {
                tarefaExistente.UsuarioId = tarefaDto.UsuarioId.Value;
            }

            var tarefaAtualizada = _tarefaRepository.Atualizar(tarefaExistente);

            return _mapper.Map<TarefaDto?>(tarefaAtualizada);
        }

        public bool Deletar(int id)
        {
            var tarefaExistente = _tarefaRepository.ObterPorId(id);

            if (tarefaExistente == null)
            {
                return false;
            }

            _tarefaRepository.Deletar(tarefaExistente);
            return true;
        }

        public bool MarcarComoConcluida(int id)
        {
            var tarefa = _tarefaRepository.ObterPorId(id);

            if (tarefa == null || tarefa.Concluida)
            {
                return false;
            }

            tarefa.Concluida = true;
            tarefa.DataConclusao = DateTimeOffset.UtcNow;

            return _tarefaRepository.SalvarAlteracoes();
        }

        public bool AssociarTag(int tarefaId, int tagId)
        {
            var tarefa = _tarefaRepository.ObterPorId(tarefaId);
            var tag = _tagRepository.ObterPorId(tagId);

            if (tarefa == null || tag == null)
            {
                return false; 
            }

            var associacaoExistente = tarefa.TarefasTags.Any(tt => tt.TagId == tagId);
            if (associacaoExistente)
            {
                return true;
            }

            var tarefaTag = new TarefaTag
            {
                TarefaId = tarefaId,
                TagId = tagId
            };

            _tarefaRepository.AdicionarTarefaTag(tarefaTag);
            return true;
        }
    }
}