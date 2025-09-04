using System;

namespace aula5;

public class Cachorro : Animal
{
    public string Raca { get; set; }

    public Cachorro(string nome, int idade, string raca) : base(nome, idade, "Cachorro")
    {
        Raca = raca;
        Console.WriteLine($"Construtor Cachorro: '{Nome}' ({Raca}) criado.");
    }

    public void Latir()
    {
        Console.WriteLine($"{Nome} está latindo");
    }

    public override void EmitirSom()
    {
        Console.WriteLine($"{Nome} da raça {Raca} está latindo alto");
    }
}
