using System;
using System.ComponentModel.DataAnnotations.Schema;
using ITB.Domain.Core.Exceptions;

namespace ITB.Domain.Entities;

public class Veiculo
{
    public int Id { get; set;}
    public string Modelo { get; set;}
    public string Placa { get; set;}
    public int Ano { get; set;}
    public int MarcaId { get; set;}
    
    public virtual Marca Marca { get; private set; }

    public Veiculo (string modelo, string placa, int ano, int marcaId)
    {
        if(string.IsNullOrEmpty(modelo))
        throw new DomainException("modelo é obrigatódio");

        if(placa.Length != 7)
        throw new DomainException("placa inválida");

        if(ano > DateTime.Now.Year + 1 || ano < 1900)
        throw new DomainException("Ano do veículo não compatível");

        if (marcaId <=0) throw new DomainException("A marca é obrigatória");

        MarcaId = marcaId;
        Modelo  = modelo;
        Placa = placa;
        Ano =ano;
    }

    protected Veiculo(){}

    public void AlterarPlaca(string novaPlaca)
    {
        if(string.IsNullOrWhiteSpace(novaPlaca))
        throw new DomainException("Placa é inválida");
    }

    public Veiculo(string modelo, int marcaId)
    {
        if (marcaId <= 0) throw new DomainException("A Marca é obrigatória.");
        Modelo = modelo;
        MarcaId = marcaId;
    }

}
