// See https://aka.ms/new-console-template for more information
// Console.WriteLine("Exercício 10/07");
namespace exercicio;

class Program
{
    public static void Main(string[] args)
    {
        Produto produto1 = new("mouse", 80.00, 10);
        Produto produto2 = new("teclado", 129.90, 15);

        produto1.ExibirDetalhes();
        produto2.ExibirDetalhes();

        produto1.AdicionarEstoque(5);

        produto2.Vender(10);
        produto2.Vender(8);
    }

}