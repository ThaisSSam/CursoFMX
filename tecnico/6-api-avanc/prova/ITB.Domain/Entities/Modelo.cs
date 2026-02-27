using System;
using ITB.Domain.Core.Exceptions;

namespace ITB.Domain.Entities;

public class Modelo
{
    public int Id { get; set; }

    public string Nome { get; set; }

    public int MarcaId { get; private set;}

    public bool Ativo { get; private set;}= true;

    public virtual Marca Marca { get; private set; }
    protected Modelo(){}

    public Modelo (string nome, int marcaId, bool ativo)
    {
        ValidarDados(nome, marcaId, ativo);

        MarcaId = marcaId;
        Nome = nome;
        Ativo = ativo;
    }

    private void ValidarDados(string nome, int marcaId, bool ativo)
    {
        if( string.IsNullOrEmpty(nome) ) throw new DomainException("Nome obrigatório");

        if (marcaId <=0) throw new DomainException("A marca é obrigatória");

        if(!ativo == null) throw new DomainException("Ativo obrigatório");
    }

    public void AlterarMarca(int novaMarcaId,bool novaMarcaExiste)
    {
        if (MarcaId == novaMarcaId) return;

        if(novaMarcaId <=0)
            throw new DomainException("A nova marca informada é inválida");

        if(!novaMarcaExiste)
            throw new DomainException("A nova marca informada não existe no sistema");
        
        MarcaId = novaMarcaId;
    }
}


