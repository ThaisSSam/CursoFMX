using GerenciadorTarefasApi.Entities;
using GerenciadorTarefasApi.Infra.Context;
using GerenciadorTarefasApi.Infra.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace GerenciadorTarefasApi.Infra.Repositories
{
    public class UsuarioDBRepository : IUsuarioRepository
    {
        private readonly TarefaContext _context;

        public UsuarioDBRepository(TarefaContext context)
        {
            _context = context;
        }

        public List<Usuario> ObterTodos()
        {
            return _context.Usuarios.ToList();
        }

        public Usuario? ObterPorId(int id, bool incluirTarefas = false)
        {
            var query = _context.Usuarios.AsQueryable();

            if (incluirTarefas)
            {
                query = query.Include(u => u.Tarefas.Where(t => !t.Concluida)).OrderBy(u => u.Tarefas.Count);
            }

            return query.FirstOrDefault(u => u.Id == id);
        }

        public Usuario Adicionar(Usuario novoUsuario)
        {
            _context.Usuarios.Add(novoUsuario);
            _context.SaveChanges();
            return novoUsuario;
        }

        public Usuario? Atualizar(Usuario usuario)
        {
            _context.Usuarios.Update(usuario);
            _context.SaveChanges();
            return usuario;
        }

        public void Deletar(Usuario usuario)
        {
            _context.Usuarios.Remove(usuario);
            _context.SaveChanges();
        }

        public bool SalvarAlteracoes()
        {
            return _context.SaveChanges() > 0;
        }
    }
}