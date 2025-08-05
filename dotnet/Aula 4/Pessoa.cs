using System;

namespace Aula_4;

public class Pessoa
{
    // atalho digite prop
    public string Nome { get; set; }
    public int Idade { get; set; }
    public double Altura { get; set; }

    public Email Email{ get; set; } 

    // atalho digite ctor
    public Pessoa()
    {
        Console.WriteLine("Construtor padrão de pessoa foi chamado");
        // valores padrões
        Nome = "Pessoa Desconhecida";
        Idade = 0;
        Altura = 0.0;
    }

    public Pessoa(string nome, int idade)
    {
        Console.WriteLine("Construtor Pessoa sem altura");
        Nome = nome;
        Idade = idade;
        Altura = 0.0;
    }

    public Pessoa(string nome, int idade, double altura)
    {
        Console.WriteLine("Construtor pessoa completo");
        this.Nome = nome;
        Idade = idade;
        Altura = altura;
    }

    // Métodos

    public void Apresentar()
    {
        Console.WriteLine($"Olá, meu nome é {Nome}, tenho {Idade} anos e {Altura:F2}m de altura");
        // F2 formata para 2 casas decimais
    }

    public void Envelhecer()
    {
        Idade = Idade + 1;
        Console.WriteLine($"{Nome} envelheceu um ano! Agora tem {Idade} anos.");
    }

    public bool EhMaiorDeIdade()
    {
        return Idade >= 18;
    }

    // sobrescrever o método base
    public override string ToString()
    {
        return $"Nome: {Nome} \n Idade: {Idade}\n Altura: {Altura}";
    }

}
