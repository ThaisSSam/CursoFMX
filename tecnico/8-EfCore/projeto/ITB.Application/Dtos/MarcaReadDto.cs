using ITB.Domain.Entities;

namespace ITB.Application.Dtos;

public class MarcaReadDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public bool Ativo { get; set; }
    public IEnumerable<Veiculo> Veiculos;
}
