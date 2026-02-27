using System;
using System.ComponentModel.DataAnnotations.Schema;
using ITB.Domain.Core.Exceptions;

namespace ITB.Domain.Entities;

public class Veiculo
{
    public int Id { get; set;}
    public string Placa { get; set;}
    public int Ano { get; set;}
    public int ModeloId { get; private set;}
    public bool Ativo { get; private set;}= true;
    
    // public virtual Marca Marca { get; private set; }
    public virtual Modelo Modelo { get; private set; }

    protected Veiculo(){}
    public Veiculo (string placa, int ano, int modeloId)
    {
        ValidarPlaca(placa);
        ValidarDados(ano);
        // if(string.IsNullOrEmpty(modelo))
        // throw new DomainException("modelo é obrigatódio");

        // if(placa.Length != 7)
        // throw new DomainException("placa inválida");

        // if(ano > DateTime.Now.Year + 1 || ano < 1900)
        // throw new DomainException("Ano do veículo não compatível");

        if (modeloId <=0) throw new DomainException("A modelo é obrigatória");

        Placa = placa;
        Ano =ano;
        ModeloId = modeloId;
    }

    public void AtualizarDados(int modeloId, int ano)
    {
        if(Ano ==ano) return;
        ValidarDados(ano);
        Ano= ano;
        ModeloId= modeloId;
    }
    public void AlterarPlaca(string novaPlaca, bool placaJaExisteEmOutroVeiculo)
    {
        if(Placa ==novaPlaca) return;

        if(placaJaExisteEmOutroVeiculo)
            throw new DomainException("Esta placa já existe em outro veículo");

        ValidarPlaca(novaPlaca);
        Placa = novaPlaca;  
    }

    public void AlterarModelo(int novaModeloId,bool novaModeloExiste)
    {
        if (ModeloId == novaModeloId) return;

        if(novaModeloId <=0)
            throw new DomainException("A nova modelo informada é inválida");

        if(!novaModeloExiste)
            throw new DomainException("A nova modelo informada não existe no sistema");
        
        ModeloId = novaModeloId;
    }

    public void Desativar()
    {
        if(!Ativo) return;
        Ativo = false;
    }

    private void ValidarPlaca(string placa)
    {
        if(string.IsNullOrWhiteSpace(placa) || placa.Length != 7)
            throw new DomainException("Formato de placa inválido");
    }

    private void ValidarDados(int ano)
    {
        // if(string.IsNullOrWhiteSpace(modelo))
        //     throw new DomainException("Modelo obrigatório");
        if(ano < 1900 || ano > DateTime.Now.Year + 1)
            throw new DomainException("Ano inválido");
    }
    // public Veiculo(string modelo, int marcaId)
    // {
    //     if (marcaId <= 0) throw new DomainException("A Marca é obrigatória.");
    //     Modelo = modelo;
    //     MarcaId = marcaId;
    // }

}
