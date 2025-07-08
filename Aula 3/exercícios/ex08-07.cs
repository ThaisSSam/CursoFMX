using System.Diagnostics;
using System.Linq.Expressions;

class Program
{
    public static void ExibirMenu()
    {
        Console.WriteLine("Digite a opção para fazer uma equação:\n");
        Console.WriteLine("1-Soma\n");
        Console.WriteLine("2-Multiplicação\n");
        Console.WriteLine("3-Subtração\n");
        Console.WriteLine("3-Divisão");
    }

    public static double ObterNumero(string prompt)
    {
        Console.WriteLine(prompt);
        double numero = Convert.ToDouble(Console.ReadLine());
        return numero;
    }

    public static double SomarNumeros(double n1, double n2)
    {
        double resultado = n1 + n2;
        return resultado;
    }

    public static double MultiplicarNumeros(double n1, double n2)
    {
        double resultado = n1 * n2;
        return resultado;
    }
    public static double SubtrairNumeros(double n1, double n2)
    {
        double resultado = n1 - n2;
        return resultado;
    }
    public static double DividirNumeros(double n1, double n2)
    {
        double resultado;
        if (n2 == 0)
        {
            Console.WriteLine("Não é possível fazer essa operação");
            resultado = 0;
        }
        else
        {
            resultado = n1 / n2;
        }
        return resultado;

    }
    static void Main(string[] args)
    {
        ExibirMenu();
        string Opcao = Console.ReadLine();
        do
        {
            string promptN1 = "Digite o primeiro número: ";
            double n1 = ObterNumero(promptN1);
            string promptN2 = "Digite o segundo número: ";
            double n2 = ObterNumero(promptN2);
            switch (Opcao)
            {
                case "1":
                    Console.WriteLine($"Resultado: {SomarNumeros(n1, n2)}");
                    break;

                case "2":
                    Console.WriteLine($"Resultado: {MultiplicarNumeros(n1, n2)}");
                    break;

                case "3":
                    Console.WriteLine($"Resultado: {SubtrairNumeros(n1, n2)}");
                    break;

                case "4":
                    Console.WriteLine($"Resultado: {DividirNumeros(n1, n2)}");
                    break;

                case "S":
                default:
                    Console.WriteLine("Digite apenas as opções: \n1-Soma\n2Multiplicação\n3-Subtração\n4-Divisão\nS-Sair");
                    break;
            }

        } while (Opcao == "S");
    }
}
