using System;

namespace docker.exercicio;

public class Vendedor : Funcionario
{
    public decimal ComissaoPorVenda { get; set; }
    private int _totalVendas { get; set; }

    public Vendedor(string nome, decimal salarioBase, decimal comissaoPorVenda, int totalVendas) : base(nome, salarioBase)
    {
        Nome = nome;
        SalarioBase = salarioBase;
        ComissaoPorVenda = comissaoPorVenda;
        _totalVendas = totalVendas;
    }

    public void RegistrarVenda(int quantidade)
    {
        _totalVendas += quantidade;
    }

    public override void ExibirDetalhes()
    {
        Console.WriteLine($"Nome: {Nome}\nSalário base: {SalarioBase}\nComissão por venda: {ComissaoPorVenda}\nTotal de vendas: {_totalVendas}");
    }

    public override decimal CalcularSalarioTotal()
    {
        SalarioBase += _totalVendas * ComissaoPorVenda;
        return SalarioBase;
    }
}
