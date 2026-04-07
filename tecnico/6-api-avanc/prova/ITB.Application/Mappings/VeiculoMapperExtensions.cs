using System;
using ITB.Application.Commands;
using ITB.Application.Dtos;
using ITB.Domain.Entities;

namespace ITB.Application.Mappings;

/// <summary>
/// Classe estática que concentra todas as regras de conversão (De/Para) do domínio de Veículos.
/// Substitui o AutoMapper para garantir alta performance e segurança em tempo de compilação.
/// </summary>
public static class VeiculoMapperExtensions
{
    /// <summary>
    /// ENTRADA: Converte o Request específico (que vem da web) para a intenção de negócio (Command).
    /// A palavra 'this' permite chamar 'request.ToCommand()' direto na Controller.
    /// </summary>
    public static AdicionarVeiculoCommand ToCommand(this AdicionarVeiculoRequest request)
    {
        return new AdicionarVeiculoCommand
        {
            modeloId = int.Parse(request.Modelo),
            placa = request.Placa,
            ano = request.Ano,
            marcaId = request.MarcaId,
            precoCusto = request.PrecoCusto,
            precoVenda = request.precoVenda

            // Observe que a propriedade 'IdGerado' não é mapeada aqui.
            // Ela existe no Command, mas não existe no Request. Isso garante que um usuário
            // mal intencionado não consiga injetar um ID falso pela requisição web!
        };
    }

    /// <summary>
    /// SAÍDA: Converte a Entidade rica do banco para o DTO de tela (Achatamento).
    /// </summary>
    public static VeiculosListagemDTO ToListagemDto(this Veiculo entidade)
    {
        return new VeiculosListagemDTO
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
    public static IEnumerable<VeiculosListagemDTO> ToListagemDtoList(this IEnumerable<Veiculo> entidades)
    {
        return entidades.Select(e => e.ToListagemDto());
    }
}