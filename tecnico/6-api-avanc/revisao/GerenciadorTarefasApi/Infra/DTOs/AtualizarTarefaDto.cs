using System.ComponentModel.DataAnnotations;

namespace GerenciadorTarefasApi.Infra.DTOs 
{
    public class AtualizarTarefaDto
    {
        [Required(ErrorMessage = "O título é obrigatório.")]
        [StringLength(100, ErrorMessage = "O título deve ter no máximo 100 caracteres.")]
        public string Titulo { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres.")]
        public string? Descricao { get; set; }

        public bool Concluida { get; set; }
        
        public int? UsuarioId { get; set; }
    }
}