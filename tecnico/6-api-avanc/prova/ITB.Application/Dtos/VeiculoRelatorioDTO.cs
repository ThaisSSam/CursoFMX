using System;

namespace ITB.Application.Dtos;

public class VeiculoRelatorioDTO
{
    //  Modelo, Ano, PrecoAtualizado)
    public string nomeMarca { get; set;} = string.Empty;

    public string Modelo { get; set;} = string.Empty;

    public int Ano { get; set;}

    public string Placa { get; set;} = string.Empty;

    public int Id { get; set;}

    public decimal PrecoAtualizado { get; set;}

    public int numeroPagina { get; set;}

    public int tamanhoPagina { get; set;}
}
