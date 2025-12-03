using System.Collections.Generic;

namespace GerenciadorTarefasApi.Infra.DTOs
{
    public class UsuarioDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public ICollection<TarefaDtoSimples> Tarefas { get; set; } = new List<TarefaDtoSimples>();
    }
   public class TarefaDtoSimples
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public bool Concluida { get; set; }
    }
}