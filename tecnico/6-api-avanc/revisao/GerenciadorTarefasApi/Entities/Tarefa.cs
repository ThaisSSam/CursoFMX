using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GerenciadorTarefasApi.Entities
{
    public class Tarefa
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public DateTimeOffset DataCriacao { get; set; }
        public DateTimeOffset? DataConclusao { get; set; }
        public bool Concluida { get; set; }
        public int UsuarioId { get; set; }

        public Usuario Usuario { get; set; } = null!;


        public DetalhesTarefa? Detalhes { get; set; }
        public ICollection<TarefaTag> TarefasTags { get; set; } = new List<TarefaTag>();
    }
}