using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace GerenciadorTarefasApi.Entities
{
    public class Tag
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public ICollection<TarefaTag> TarefasTags { get; set; } = new List<TarefaTag>();
    }
}
