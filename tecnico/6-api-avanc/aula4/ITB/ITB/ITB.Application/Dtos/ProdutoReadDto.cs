using System;
using ITB.Domain.Entities;

namespace ITB.Application.Dtos;

public class ProdutoReadDto
{
    public int id { get; set; }

    public string nome { get; set;} 

    public int preco{ get; set;}
    public int fabricanteId { get; set; }
    public Fabricante? fabricante { get; set; }
}
