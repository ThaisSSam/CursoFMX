using System.Reflection.Metadata;

namespace ITB.Application.Dtos;

public class VeiculoSimplesDto
{
    public int Id { get; set; }
    public string Placa { get; set; } = string.Empty;
    public string Modelo { get; set; } = string.Empty;
    public int Ano { get; set; }
}

public class MarcaComVeiculoDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public bool Ativo { get; set; }
    public List<VeiculoSimplesDto> Veiculos { get; set; } = new();
}
