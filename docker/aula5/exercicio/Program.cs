namespace aual5;

public class Program
{
    public static void Main(string[] args)
    {
        List<string> listaFuncionarios = new List<string>();
        Funcionario funcionario1 = new Funcionario("Pedro", 5000, 00);
        listaFuncionarios.add(funcionario1);

        Gerente gerente1 = new Gerente("Maria", 6000, "TI");
        listaFuncionarios.add(gerente1);

        Vendedor vendedor1 = new Vendedor("Fabio", 3000, 10, 5);
        listaFuncionarios.add(vendedor1);

        foreach (var funcionario in listaFuncionarios)
        {
            Console.WriteLine($"Detalhes:\n {funcionario.ExibirDetalhes()}");
            Console.WriteLine($"Salario total:\n {funcionario.CalcularSalarioTotal()}");
        }
    }
}