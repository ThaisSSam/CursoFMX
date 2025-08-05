using System;
using System.Numerics;

namespace exercicio;

public class Produto
{
    public string Nome { get; set; }

    public double Preco { get; set; }

    public int Estoque { get; set; }

    public Produto(string nome, double preco, int estoque)
    {
        Nome = nome;
        Preco = preco;
        Estoque = estoque;
    }

    public void ExibirDetalhes()
    {
        Console.WriteLine($"\nDetalhes do produto:\nNome: {Nome}\nPreço: R${Preco:F2}\nEstoque: {Estoque}");
    }

    public void AdicionarEstoque(int quantidade)
    {
        Console.WriteLine($"\n Quantidade de estoque: {Estoque}");
        Console.WriteLine($"\nQuantidade adicionada ao estoque: {quantidade}");
        Estoque += quantidade;
        Console.WriteLine($"Novo estoque: {Estoque}");
    }

    public bool Vender(int quantidade)
    {
        Console.WriteLine($"\n Quantidade de estoque: {Estoque}");
        Console.WriteLine($"\nQuantidade vendida: {quantidade}");
        if (Estoque >= quantidade)
        {
            Estoque -= quantidade;
            Console.WriteLine($"Novo estoque: {Estoque}");
            return true;
        }
        else
        {
            Console.WriteLine("Estoque insuficiente");
            return false;
        }
    }
}
