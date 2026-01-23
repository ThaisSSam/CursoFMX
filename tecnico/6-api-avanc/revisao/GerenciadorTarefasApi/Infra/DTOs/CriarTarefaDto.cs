using System;
using System.ComponentModel.DataAnnotations;

namespace GerenciadorTarefasApi.Infra.DTOs;

public class CriarTarefaDto
{
    [Required]
    [StringLength(50)]
    public string Titulo { get; set; } = string.Empty;
    [Required]
    [StringLength(150)]
    public string Descricao { get; set; } = string.Empty;

    [Required(ErrorMessage = "O ID do usuário é obrigatório.")] // Chave Estrangeira
    public int UsuarioId { get; set; }

}
