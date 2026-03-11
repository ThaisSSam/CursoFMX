using System;
using System.Collections.Generic;
using ITB.Domain.Core.Exceptions; // Verifique se este caminho existe mesmo

// 1. Onde começa, Cria as classes, Define os relacionamentos, Use os construtores para validar os dados. Dps vem o dbcontext
namespace ITB.Domain.Entities;

public class Marca
{
    public int Id { get; private set; } 
    public string Nome { get; private set; } = string.Empty;
    public bool Ativo { get; private set; }

    // Relacionamento: Uma Marca tem muitos Modelos
    public virtual ICollection<Modelo> Modelos { get; private set; } = new List<Modelo>();

    protected Marca() { }

    public Marca(string nome, bool ativo = true)
    {
        ValidarDados(nome);
        Nome = nome;
        Ativo = ativo;
    }

    public void AlterarNome(string novoNome)
    {
        ValidarDados(novoNome);
        Nome = novoNome;
    }

    public void Ativar() => Ativo = true;
    public void Desativar() => Ativo = false;

    private void ValidarDados(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException("O nome da marca é obrigatório.");

        if (nome.Length < 2)
            throw new DomainException("O nome da marca deve ter pelo menos 2 caracteres.");

        if (nome.Length > 50)
            throw new DomainException("O nome da marca não pode exceder 50 caracteres.");
    }
}