using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GerenciadorTarefasApi.Entities
{
    public class DetalhesTarefa
    {
        public int TarefaId { get; set; }
        public int Prioridade { get; set; } 
        public string? NotasAdicionais { get; set; }

        public Tarefa Tarefa { get; set; } = null!;
    }
}
