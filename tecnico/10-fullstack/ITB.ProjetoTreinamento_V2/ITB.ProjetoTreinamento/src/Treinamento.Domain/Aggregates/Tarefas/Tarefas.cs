using System;
using Treinamento.Domain.Aggregates.Usuarios; 
namespace Treinamento.Domain.Aggregates.Tarefa;

public class Tarefa
{
    public int Id { get; private set; } 
    public string Nome { get; private set; }
    public TipoSituacao Situacao { get; private set; }
    public TipoPrioridade Prioridade { get; private set; }
    public DateTime DataCriacao { get; private set; }
    public int UsuarioId { get; private set; }
    public virtual Usuario UsuarioResponsavel { get; private set; }

    protected Tarefa() { }

    public Tarefa(string nome, int usuarioId, TipoSituacao situacao, TipoPrioridade prioridade)
    {
        Nome = nome;
        UsuarioId = usuarioId;
        Situacao = situacao;
        Prioridade = prioridade;
        DataCriacao = DateTime.UtcNow; 
    }
}

public enum TipoSituacao
{
    Pendente = 1,
    EmAndamento = 2,
    Concluido = 3
}

public enum TipoPrioridade
{
    Baixa = 1,
    Media = 2,
    Alta = 3
}