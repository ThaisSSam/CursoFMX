using GerenciadorTarefasApi.Entities;
using GerenciadorTarefasApi.Infra.Context;
using GerenciadorTarefasApi.Infra.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace GerenciadorTarefasApi.Infra.Repositories
{
    public class TarefaDBRepository : ITarefaRepository
    {
        private readonly TarefaContext _context;

        public TarefaDBRepository(TarefaContext context)
        {
            _context = context;
        }

        public List<Tarefa> ObterTodos()
        {
            return _context.Tarefas.Include(t => t.Usuario).Include(t =>t.Detalhes).ToList();
        }

        public Tarefa? ObterPorId(int id)
        {
           
            return _context.Tarefas.Include(t => t.Usuario).Include(t => t.Detalhes).FirstOrDefault(t=> t.Id == id);
        }

        public Tarefa Adicionar(Tarefa novaTarefa)
        {
            _context.Tarefas.Add(novaTarefa);
            _context.SaveChanges();
            return novaTarefa;
        }
        public Tarefa? Atualizar(Tarefa tarefa)
        {
            _context.Tarefas.Update(tarefa);
            _context.SaveChanges();
            return tarefa;
        }
        public void Deletar(Tarefa tarefa)
        {
            _context.Tarefas.Remove(tarefa);
            _context.SaveChanges();
        }

        public bool SalvarAlteracoes()
        {
            return _context.SaveChanges() > 0;
        }


        public TarefaTag AdicionarTarefaTag(TarefaTag tarefaTag)
        {
            _context.TarefasTags.Add(tarefaTag);
            _context.SaveChanges();
            return tarefaTag;
        }
    }
}