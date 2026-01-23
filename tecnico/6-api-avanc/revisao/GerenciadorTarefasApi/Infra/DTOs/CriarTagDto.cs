using System.ComponentModel.DataAnnotations;

namespace GerenciadorTarefasApi.Infra.DTOs
{
    // A classe deve envolver as propriedades.
    public class CriarTagDto 
    {
        [Required(ErrorMessage = "O nome da Tag é obrigatório.")]
        [StringLength(50, ErrorMessage = "O nome da Tag deve ter no máximo 50 caracteres.")]
        public string Nome { get; set; } = string.Empty;
    }
}