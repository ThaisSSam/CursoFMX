using Treinamento.Domain.Core.Validacao;

namespace Treinamento.Domain.Core.Entities;

public abstract class Entity<T> where T : Entity<T>
{
    public int Id { get; protected set; }

    public abstract bool EhValido();

    public ResultadoValidacaoDominio ResultadoValidacao { get; protected set; }

    protected Entity()
    {
        ResultadoValidacao = new ResultadoValidacaoDominio();
    }

    protected void AdicionarErroValidacao(string mensagem, string? propriedade = null)
    {
        ResultadoValidacao ??= new ResultadoValidacaoDominio();
        ResultadoValidacao.Erros.Add(new ErroValidacaoDominio(mensagem, propriedade));
    }

    public override bool Equals(object? obj)
    {
        var compareTo = obj as Entity<T>;

        if (ReferenceEquals(this, compareTo)) return true;
        if (ReferenceEquals(null, compareTo)) return false;

        return Id.Equals(compareTo.Id);
    }

    public static bool operator ==(Entity<T>? a, Entity<T>? b)
    {
        if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
            return true;
        if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(Entity<T>? a, Entity<T>? b) => !(a == b);

    public override int GetHashCode() => (GetType().GetHashCode() * 907) + Id.GetHashCode();

    public override string ToString() => $"{GetType().Name}[Id = {Id}]";
}
