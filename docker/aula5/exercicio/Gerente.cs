using System;

namespace docker.exercicio;

public class Gerente : Funcionario
{
    public string Departamento { get; set; }

    public Gerente(string nome, decimal salarioBase, string departamento) : base(nome, salarioBase)
    {
        Nome = nome;
        SalarioBase = salarioBase;
        Departamento = departamento;
    }

    public override void ExibirDetalhes()
    {
        Console.WriteLine($"Nome: {Nome}\nSalário base: {SalarioBase}\nDepartamento: {Departamento}");
    }

    public override decimal CalcularSalarioTotal()
    {
        SalarioBase += SalarioBase * 0.2m;
        return SalarioBase;
    }


}
