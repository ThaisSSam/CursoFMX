using Treinamento.Domain.Core.Entities;

namespace Treinamento.Domain.Aggregates.Usuarios;

/// <summary>
/// Agregado de usuário — base do Módulo 1 (US001).
/// No boilerplate contém apenas <see cref="Id"/>; os alunos acrescentam e-mail, senha, situação, etc.
/// </summary>
public class Usuario : Entity<Usuario>
{
    public string Email { get; private set; }
    public string SenhaHash { get; private set; }
    public bool Ativo { get; private set; }
    public int TentativasLoginInvalidas { get; private set; }
    public DateTime? BloqueadoAte { get; private set; }    
    protected Usuario() : base() { }

    public Usuario(string email, string senhaHash, bool ativo) : base()
    {
        Email = email;
        SenhaHash = senhaHash;
        Ativo = ativo;
        TentativasLoginInvalidas = 0;
    }
    public override bool EhValido()
    {
        ResultadoValidacao.Erros.Clear();

        if (string.IsNullOrWhiteSpace(Email))
            AdicionarErroValidacao("E-mail é um campo obrigatório.", nameof(Email));
        else if (!Email.Contains("@"))
            AdicionarErroValidacao("Formato de e-mail inválido.", nameof(Email));

        if (string.IsNullOrWhiteSpace(SenhaHash))
            AdicionarErroValidacao("Senha é um campo obrigatório.", nameof(SenhaHash));

        return ResultadoValidacao.Erros.Count == 0;
    }

    public bool RealizarTentativaLogin(string senhaInformada, Func<string, string, bool> verificarSenhaVerdadeira)
    {
        // Verifica se o usuário já estourou o limite de 5 tentativas consecutivas
        if (TentativasLoginInvalidas >= 5)
        {
            AdicionarErroValidacao("Limite de tentativas excedido. Seu acesso foi temporariamente bloqueado.");
            return false;
        }

        // Verifica se o usuário está Inativo antes de validar a senha
        if (!Ativo)
        {
            AdicionarErroValidacao("Acesso bloqueado. Por favor, contate o administrador.");
            return false;
        }

        // Valida E-mail e Senha
        bool senhaValida = verificarSenhaVerdadeira(senhaInformada, SenhaHash);

        if (!senhaValida)
        {
            TentativasLoginInvalidas++;
            
            // ERRO
            AdicionarErroValidacao("E-mail ou senha incorretos.");
            return false;
        }

        TentativasLoginInvalidas = 0; // Reseta o contador de erros
        return true;
    }
}