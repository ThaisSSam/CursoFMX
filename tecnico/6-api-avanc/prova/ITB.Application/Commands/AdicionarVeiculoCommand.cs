using System;
using ITB.Domain.Core.Messages.Interfaces;
using ITB.Domain.Entities;


namespace ITB.Application.Commands;

public class AdicionarVeiculoCommand :ICommand
{
    public int id { get; set; }
    public string nome{get; set;} = string.Empty;
    public string placa{get; set;} = string.Empty;
    public int ano{get; set;}
    public int modeloId { get; set; }
    public int marcaId { get; set; }
    public decimal precoCusto { get; set; }
    public decimal precoVenda { get; set; }
    public int? IdGerado { get; set; }

}
