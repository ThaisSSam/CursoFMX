using System;

namespace ITB.Application.Dtos;

public class MarcaDTO
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public bool Ativo { get; set; }
}