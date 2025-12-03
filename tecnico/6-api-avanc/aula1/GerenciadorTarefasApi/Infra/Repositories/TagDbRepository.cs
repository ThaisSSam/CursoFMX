using GerenciadorTarefasApi.Entities;
using GerenciadorTarefasApi.Infra.Context;
using GerenciadorTarefasApi.Infra.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace GerenciadorTarefasApi.Infra.Repositories
{
    public class TagDBRepository : ITagRepository
    {
        private readonly TarefaContext _context;

        public TagDBRepository(TarefaContext context)
        {
            _context = context;
        }

        public List<Tag> ObterTodos()
        {
            return _context.Tags.ToList();
        }

        public Tag? ObterPorId(int id)
        {
            return _context.Tags.FirstOrDefault(t => t.Id == id);
        }

        public Tag Adicionar(Tag novaTag)
        {
            _context.Tags.Add(novaTag);
            _context.SaveChanges();
            return novaTag;
        }

        public Tag? Atualizar(Tag tag)
        {
            _context.Tags.Update(tag);
            _context.SaveChanges();
            return tag;
        }

        public void Deletar(Tag tag)
        {
            _context.Tags.Remove(tag);
            _context.SaveChanges();
        }

        public bool SalvarAlteracoes()
        {
            return _context.SaveChanges() > 0;
        }
    }
}