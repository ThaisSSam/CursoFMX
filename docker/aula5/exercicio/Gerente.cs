using System;

namespace docker.exercicio;

public class Gerente : Funcionario
{
    public string Departamento { get; set; }

    public Gerente(string nome, decimal salarioBase, string departamento) : base(nome, salarioBase)
    {
        nome = nome;
        salarioBase = salarioBase;
        Departamento = departamento;
    }

    public override void ExibirDetalhes()
    {
        Console.WriteLine($"Nome: {Nome}\nSalário base: {SalarioBase}\nDepartamento: {Departamento}");
    }

    public override decimal CalcularSalarioTotal()
    {
        salarioBase += salarioBase * 0.2;
        return salarioBase;
    }


}
