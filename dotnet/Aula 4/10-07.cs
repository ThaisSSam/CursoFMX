// See https://aka.ms/new-console-template for more information
using Aula_4;
// sem o namespace tem que usar o using
namespace Aula_4;

// dentro do solution explorer, botão direito no arquivo, new file, class, digita o nome da class

class Program
{
    public static void Main(string[] args)
    {
        Pessoa pessoa1 = new Pessoa();
        Console.WriteLine("Objeto pessoa1 criado");

        Pessoa pessoa2 = new("Nome", 29, 1.71);
        Console.WriteLine("Objeto pessoa2 criado");

        // Atribuindo valores

        pessoa1.Nome = "Alice";
        pessoa1.Idade = 30;
        pessoa1.Altura = 1.65;
        Console.WriteLine($"\nDados da pessoa1:\n {pessoa1.Nome} \n {pessoa1.Idade}\n {pessoa1.Altura:F2}m\n");

        pessoa2.Apresentar();

        Pessoa pessoa3 = new()
        {
            Nome = "Bruno",
            Idade = 19,
            Altura = 1.68
        };

        // Sobrescrito
        Console.WriteLine($"pessoa3 toString: \n {pessoa3.ToString()}");

        pessoa3.Envelhecer();

        bool brunoEhMaior = pessoa3.EhMaiorDeIdade();
        Console.WriteLine($"Bruno é maior de idade? {brunoEhMaior}");
    }
}
