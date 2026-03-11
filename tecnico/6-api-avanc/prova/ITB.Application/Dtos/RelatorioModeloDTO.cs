using System;

namespace ITB.Application.Dtos;

public class RelatorioModeloDTO
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string NomeMarca { get; set; } = string.Empty; 
    public bool Ativo { get; set; } 
    public int Quantidade { get; set; } = 0;
}
