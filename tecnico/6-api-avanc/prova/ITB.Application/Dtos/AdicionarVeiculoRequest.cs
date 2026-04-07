using System;

namespace ITB.Application.Dtos;

public class AdicionarVeiculoRequest
{
  public string Modelo { get; set; } = string.Empty;
  public string Placa { get; set; } = string.Empty;

  public int Ano { get; set; }
  public int MarcaId { get; set; }
  public decimal PrecoCusto { get; set; }
  public decimal precoVenda { get; set; }
}
