using Treinamento.Domain.Core.Entities;
namespace Treinamento.Domain.Aggregates.Usuarios;
public class Usuario : Entity<Usuario>
{
    public string Nome { get; private set; }
    public string Email { get; private set; }
    public string SenhaHash { get; private set; }
    public bool Ativo { get; private set; }
    public int TentativasLoginInvalidas { get; private set; }
    public DateTime? BloqueadoAte { get; private set; }
    protected Usuario() : base()
    {
        Nome = string.Empty;
        Email = string.Empty;
        SenhaHash = string.Empty;
    }

    public Usuario(string nome, string email, string senhaLimpa, bool ativo) : base()
    {
        Nome = nome;
        Email = email;
        SenhaHash = BCrypt.Net.BCrypt.HashPassword(senhaLimpa);
        Ativo = ativo;
        TentativasLoginInvalidas = 0;
    }

    public void AtualizarSenha(string novaSenhaLimpa)
    {
        if (string.IsNullOrWhiteSpace(novaSenhaLimpa) || novaSenhaLimpa.Length < 4)
            throw new Exception("A nova senha não atende aos requisitos mínimos.");

        SenhaHash = BCrypt.Net.BCrypt.HashPassword(novaSenhaLimpa);
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

    public bool RealizarTentativaLogin(string senhaInformada)
    {
        if (TentativasLoginInvalidas >= 5)
        {
            AdicionarErroValidacao("Limite de tentativas excedido. Seu acesso foi temporariamente bloqueado.");
            return false;
        }

        if (!Ativo)
        {
            AdicionarErroValidacao("Acesso bloqueado. Por favor, contate o administrador.");
            return false;
        }

        bool senhaValida = BCrypt.Net.BCrypt.Verify(senhaInformada.Trim(), SenhaHash);

        if (!senhaValida)
        {
            TentativasLoginInvalidas++;

            if (TentativasLoginInvalidas >= 5)
            {
                BloqueadoAte = DateTime.UtcNow.AddMinutes(15);
                AdicionarErroValidacao("Limite de tentativas excedido. Seu acesso foi temporariamente bloqueado por 15 minutos.");
                return false;
            }

            AdicionarErroValidacao("E-mail ou senha incorretos. Verifique suas credenciais e tente novamente.");
            return false;
        }

        TentativasLoginInvalidas = 0;
        BloqueadoAte = null;
        return true;
    }
}