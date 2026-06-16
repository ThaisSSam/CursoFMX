using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Treinamento.Domain.Aggregates.Tarefa;

[Table("tb_tarefas_historico", Schema = "treinamento")]
public class TarefaHistorico
{
    public int Id { get; private set; }
    public int TarefaId { get; private set; }
    public string Nome { get; private set; }
    public int Situacao { get; private set; }
    public int Prioridade { get; private set; }
    public int UsuarioId { get; private set; }
    public DateTime DataAlteracao { get; private set; }
    public string TipoAcao { get; private set; } 
    public int? UsuarioAlteracaoId { get; private set; }

    protected TarefaHistorico() { }

    public TarefaHistorico(Tarefa tarefa, string tipoAcao, int? usuarioAlteracaoId = null)
    {
        TarefaId = tarefa.Id;
        Nome = tarefa.Nome;
        Situacao = (int)tarefa.Situacao;
        Prioridade = (int)tarefa.Prioridade;
        UsuarioId = tarefa.UsuarioId;
        DataAlteracao = DateTime.UtcNow;
        TipoAcao = tipoAcao;
        UsuarioAlteracaoId = usuarioAlteracaoId;
    }
}