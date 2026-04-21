using ITB.Domain.Core;
using ITB.Domain.Core.Exceptions;

namespace ITB.Domain.Entities;

public class Usuario
{
    public int Id { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string SenhaHash { get; private set; } = string.Empty; 
    public string Perfil { get; private set; } = string.Empty; // "Gerente" ou "Vendedor"
    
    // Trava de segurança pós-reset
    public bool PrecisaTrocarSenha { get; private set; }

    protected Usuario() { }

    // 1. O CONSTRUTOR É PRIVADO (Proteção total)
    private Usuario(string nome, string email, string senhaClara, string perfil)
    {
        Nome = nome;
        Email = email;
        Perfil = perfil;
        PrecisaTrocarSenha = false; // Nasce como false, pois a senha foi escolhida por ele
        SetSenhaUnsafe(senhaClara); // Criptografa imediatamente!
    }

    // 2. O FACTORY METHOD (A Fábrica que usa o Result Pattern)
    public static Result<Usuario> Criar(string nome, string email, string senhaClara, string perfil)
    {
        var erros = new List<string>();

        if (string.IsNullOrWhiteSpace(nome)) erros.Add("Nome é obrigatório.");
        if (string.IsNullOrWhiteSpace(email)) erros.Add("E-mail é obrigatório.");
        if (string.IsNullOrWhiteSpace(senhaClara) || senhaClara.Length < 6) 
            erros.Add("Senha deve ter no mínimo 6 caracteres.");
        if (string.IsNullOrWhiteSpace(perfil) || (perfil != "Gerente" && perfil != "Vendedor"))
            erros.Add("Perfil inválido. Deve ser 'Gerente' ou 'Vendedor'.");

        if (erros.Count > 0) return Result<Usuario>.Failure(erros);

        return Result<Usuario>.Success(new Usuario(nome, email, senhaClara, perfil));
    }

    // 3. SEGURANÇA E COMPORTAMENTOS (Sem Exceptions!)
    private void SetSenhaUnsafe(string senhaClara)
    {
        SenhaHash = BCrypt.Net.BCrypt.HashPassword(senhaClara);
    }

    public bool ValidarSenha(string senhaTentativa)
    {
        return BCrypt.Net.BCrypt.Verify(senhaTentativa, SenhaHash);
    }

    public Result AlterarSenha(string senhaAtual, string novaSenha)
    {
        var erros = new List<string>();

        if (!ValidarSenha(senhaAtual))
            erros.Add("A senha atual informada está incorreta.");

        if (string.IsNullOrWhiteSpace(novaSenha) || novaSenha.Length < 6)
            erros.Add("A nova senha deve ter no mínimo 6 caracteres.");

        if (erros.Count > 0)
            return Result.Failure(erros);

        SetSenhaUnsafe(novaSenha);
        PrecisaTrocarSenha = false; // Trava liberada!
        return Result.Success();
    }

    public string ResetarSenha()
    {
        var senhaProvisoria = Guid.NewGuid().ToString().Substring(0, 8);
        SetSenhaUnsafe(senhaProvisoria);
        PrecisaTrocarSenha = true; // Trava ativada! 
        return senhaProvisoria; 
    }
}