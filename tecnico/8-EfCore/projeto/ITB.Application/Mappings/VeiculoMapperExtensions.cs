using ITB.Application.Commands;
using ITB.Application.Dtos;
using ITB.Domain.Entities;

/// <summary>
/// Classe estática que concentra todas as regras de conversão (De/Para) do domínio de Veículos.
/// Substitui o AutoMapper para garantir alta performance e segurança em tempo de compilação.
/// </summary>
public static class VeiculoMapperExtensions
{
    public static AtualizarVeiculoCommand ToCommand(this AtualizarVeiculoRequest request, int id)
    {
        return new AtualizarVeiculoCommand
        {
            Id = id,
            Nome = request.Nome,
            ModeloId = request.ModeloId,
            Placa = request.Placa,
            Ano = request.Ano,
            MarcaId = request.MarcaId,
            PrecoCusto = request.PrecoCusto,
            PrecoVenda = request.PrecoVenda

            // Observe que a propriedade 'IdGerado' não é mapeada aqui.
            // Ela existe no Command, mas não existe no Request. Isso garante que um usuário
            // mal intencionado não consiga injetar um ID falso pela requisição web!
        };
    }
    /// <summary>
    /// ENTRADA: Converte o Request específico (que vem da web) para a intenção de negócio (Command).
    /// A palavra 'this' permite chamar 'request.ToCommand()' direto na Controller.
    /// </summary>
    public static AdicionarVeiculoCommand ToCommand(this AdicionarVeiculoRequest request)
    {
        return new AdicionarVeiculoCommand
        {
            Nome = request.Nome,
            ModeloId = request.ModeloId,
            Placa = request.Placa,
            Ano = request.Ano,
            MarcaId = request.MarcaId,
            PrecoCusto = request.PrecoCusto,
            PrecoVenda = request.PrecoVenda

            // Observe que a propriedade 'IdGerado' não é mapeada aqui.
            // Ela existe no Command, mas não existe no Request. Isso garante que um usuário
            // mal intencionado não consiga injetar um ID falso pela requisição web!
        };
    }

    /// <summary>
    /// SAÍDA: Converte a Entidade rica do banco para o DTO de tela (Achatamento).
    /// </summary>
    public static VeiculoListagemDto ToListagemDto(this Veiculo entidade)
    {
        return new VeiculoListagemDto
        {
            Id = entidade.Id,
            Modelo = entidade.Modelo?.Nome ?? string.Empty,
            Placa = entidade.Placa,
            Ano = entidade.Ano,
            PrecoCusto = entidade.PrecoCusto,
            PrecoVenda = entidade.PrecoVenda,
            
            // ACHATAMENTO (Flattening): Pegamos a propriedade "Nome" de dentro do objeto Marca.
            // O uso do operador '?' evita erros (NullReferenceException) caso a query no banco 
            // não tenha feito o Join/Include com a tabela de Marcas.
            Marca = entidade.Marca?.Nome ?? string.Empty 
        };
    }

    /// <summary>
    /// SAÍDA EM MASSA: Aplica o mapeamento de tela em uma lista inteira.
    /// </summary>
    public static IEnumerable<VeiculoListagemDto> ToListagemDtoList(this IEnumerable<Veiculo> entidades)
    {
        return entidades.Select(e => e.ToListagemDto());
    }
}