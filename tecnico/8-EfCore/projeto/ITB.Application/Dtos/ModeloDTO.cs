using ITB.Domain.Entities;

namespace ITB.Application.Dtos;

public class ModeloDTO
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int MarcaId { get; set; }
    public MarcaDTO Marca { get; set; }
    public bool Ativo { get; set; }
}