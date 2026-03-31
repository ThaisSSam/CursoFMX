using System;

namespace ITB.Domain.Entities;

public class Usuario
{
  public int id { get; set; }

  public string name { get; set; } = string.Empty;

  public string email { get; set; } = string.Empty;

  public string senha { get; set; } = string.Empty;

  public string perfil { get; set; } = string.Empty;
}
