using System;

namespace aula5;

public class Gato : Animal // Gato HERDA de Animal
{
    public string CorPelo { get; set; }

    public Gato(string nome, int idade, string corPelo)
        : base(nome, idade, "Gato") // Chama o construtor da classe base 'Animal'
    {
        CorPelo = corPelo;
        Console.WriteLine($"Construtor Gato: '{Nome}' ({CorPelo}) criado.");
    }

    public void Miar()
    {
        Console.WriteLine($"{Nome} está miando: Miau!");
    }

    // 'override' modifica o comportamento herdado de EmitirSom() (Polimorfismo!)
    public override void EmitirSom()
    {
        Console.WriteLine($"{Nome} ({CorPelo}) está miando suavemente: miau miau.");
    }
}
 