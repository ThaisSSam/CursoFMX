using System.Collections.Generic;

namespace GerenciadorTarefasApi.Infra.DTOs
{
    public class TagDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
    }
}