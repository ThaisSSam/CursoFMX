using System;
using Treinamento.Domain.Aggregates.Usuarios; // Certifique-se de importar a pasta do Usuário

namespace Treinamento.Domain.Aggregates.Tarefas;

public class Tarefa
{
    public int Id { get; private set; } // O seu "cód"
    public string Nome { get; private set; }
    
    // 1. Mudança aqui: Guardamos o ID do responsável e a propriedade de navegação do EF
    public int UsuarioId { get; private set; } 
    public virtual Usuario UsuarioResponsavel { get; private set; }

    public SituacaoTarefa Situacao { get; private set; }
    public PrioridadeTarefa Prioridade { get; private set; }
    public DateTime DataCriacao { get; private set; }

    protected Tarefa() { }

    // Construtor atualizado recebendo o id do responsável
    public Tarefa(string nome, int usuarioId, PrioridadeTarefa prioridade)
    {
        if (string.IsNullOrWhiteSpace(nome)) 
            throw new ArgumentException("O nome da tarefa é obrigatório.");
            
        if (usuarioId <= 0) 
            throw new ArgumentException("Um responsável válido deve ser informado.");

        Nome = nome;
        UsuarioId = usuarioId;
        Prioridade = prioridade;
        Situacao = SituacaoTarefa.Aberto;
        DataCriacao = DateTime.UtcNow;
    }

    public void AlterarSituacao(SituacaoTarefa novaSituacao) => Situacao = novaSituacao;
    public void AlterarPrioridade(PrioridadeTarefa novaPrioridade) => Prioridade = novaPrioridade;
}