using System;
using ITB.Domain.Core.Exceptions;

namespace ITB.Domain.Entities;

public class Usuario
{
  public int id { get; set; }

  public string name { get; set; } = string.Empty;

  public string email { get; set; } = string.Empty;

  public string senha { get; set; } = string.Empty;

  public string perfil { get; set; } = string.Empty;

protected Usuario(){}

public Usuario(string nome, string email, string senha, string perfil)
    {
        ValidarDados(nome, email, senha, perfil);

        this.name = nome;
        this.email = email;
        this.senha = senha;
        this.perfil = perfil;
    }

    private void ValidarDados(string nome, string email, string senha, string perfil)
    {
        if (string.IsNullOrWhiteSpace(nome)) throw new DomainException("Nome é obrigatório.");
        if (string.IsNullOrWhiteSpace(email)) throw new DomainException("E-mail é obrigatório.");
        if (string.IsNullOrWhiteSpace(senha)) throw new DomainException("Senha é obrigatória.");

        if (string.IsNullOrWhiteSpace(perfil) || (perfil != "Gerente" && perfil != "Vendedor"))
            throw new DomainException("Perfil inválido. Deve ser 'Gerente' ou 'Vendedor'.");
    }
}