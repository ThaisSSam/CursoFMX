using System;
using System.ComponentModel.DataAnnotations;

namespace GerenciadorTarefasApi.Infra.DTOs
{
    public class TarefaDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string? Descricao { get; set; } = string.Empty;
        public DateTimeOffset DataCriacao { get; set; }
        public DateTimeOffset? DataConclusao { get; set; }

        public bool Concluida { get; set; }

        public UsuarioResumoDto Usuario { get; set; }= new UsuarioResumoDto();
        public DetalhesTarefaDto? Detalhes { get; set; }
    }
    public class UsuarioResumoDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } =string.Empty;
    }

    public class DetalhesTarefaDto
    {
        public int Prioridade { get; set; }
        public string? NotasAdicionais { get; set; }
    }
}

