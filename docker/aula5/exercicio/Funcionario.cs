using System;

namespace docker.exercicio;

public class Funcionario
{
    public string Nome { get; set; }
    public decimal SalarioBase { get; set; }

    public Funcionario(string nome, decimal salarioBase)
    {
        Nome = nome;
        SalarioBase = salarioBase;
    }

    public virtual void ExibirDetalhes()
    {
        Console.WriteLine($"Nome: {Nome}\nSalário base: {SalarioBase}");
    }

    public virtual decimal CalcularSalarioTotal()
    {
        return SalarioBase;
    }

}
