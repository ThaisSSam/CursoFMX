using System;
using docker.exercicio;


public class Program
{
    public static void Main(string[] args)
    {
        List<Funcionario> listaFuncionarios = new List<Funcionario>();
        Funcionario funcionario1 = new Funcionario("Pedro", 5000);
        listaFuncionarios.Add(funcionario1);

        Gerente gerente1 = new Gerente("Maria", 6000, "TI");
        listaFuncionarios.Add(gerente1);

        Vendedor vendedor1 = new Vendedor("Fabio", 3000, 10, 5);
        listaFuncionarios.Add(vendedor1);

        foreach (var funcionario in listaFuncionarios)
        {
            funcionario.ExibirDetalhes();
            Console.WriteLine($"Salario total: {funcionario.CalcularSalarioTotal()}\n");
        }
    }
}