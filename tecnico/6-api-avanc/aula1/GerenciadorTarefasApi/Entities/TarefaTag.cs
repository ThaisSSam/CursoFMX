using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace GerenciadorTarefasApi.Entities
{
    public class TarefaTag
    {
        public int TarefaId { get; set; }
        public int TagId { get; set; }

        public Tarefa Tarefa { get; set; } = null!;
        public Tag Tag { get; set; } = null!;
    }
}