using System;
using ITB.Domain.Entities;

namespace ITB.Application.Dtos;

public class ModeloDTO
{
    public int Id { get; set; }

    public string Nome { get; set; }

    public bool Ativo { get; set; }
    public MarcaDTO? Marca { get; set; }
}
