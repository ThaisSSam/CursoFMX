using System;
using ITB.Domain.Core.Exceptions;

namespace ITB.Domain.Entities;

public class Veiculo
{
    public int Id { get; set; }
    public string Placa { get; set; }
    public int Ano { get; set; }
    
    // 1. Garanta que o EF consiga gravar os IDs diretamente
    public int ModeloId { get; set; } 
    public int MarcaId { get; set; } 

    public decimal Preco { get; set; }
    public bool Ativo { get; private set; } = true;
    public decimal PrecoCusto { get; set; }
    public decimal PrecoVenda { get; set; }

    // Propriedades de Navegação
    public virtual Marca Marca { get; private set; }
    public virtual Modelo Modelo { get; private set; }

  // Token de Concorrência (xmin do Postgres mapeado no Contexto)
  public uint VersaoLinha { get; set; }

  // Construtor vazio para o Entity Framework
  protected Veiculo() { }

  // Construtor Principal: Usado pelo AdicionarVeiculoHandler
  public Veiculo(string placa, int ano, int modeloId, decimal precoCusto, decimal precoVenda, int marcaId)
  {
    ValidarPlaca(placa);
    ValidarDados(ano);
    ValidarPrecos(precoCusto, precoVenda);

    if (modeloId <= 0)
      throw new DomainException("O modelo é obrigatório.");
    // Mudar isso para marcaId
    // if (marcaId <= 0) 
    //     throw new DomainException("O modelo é obrigatório.");

    Placa = placa;
    Ano = ano;
    ModeloId = modeloId;
    PrecoCusto = precoCusto;
    PrecoVenda = precoVenda;
    MarcaId = marcaId;
  }

  public Veiculo(int modeloId, string placa, int ano, int marcaId, decimal precoCusto, decimal precoVenda)
  {
    ModeloId = modeloId;
    Placa = placa;
    Ano = ano;
    MarcaId = marcaId;
    PrecoCusto = precoCusto;
    PrecoVenda = precoVenda;
  }

  // Métodos de Negócio (Comportamentos)
  public void AtualizarDados(int modeloId, int ano)
  {
    ValidarDados(ano);
    if (modeloId <= 0) throw new DomainException("Modelo inválido");

    Ano = ano;
    ModeloId = modeloId;
  }

  public void AtualizarPrecos(decimal precoCusto, decimal precoVenda)
  {
    if (PrecoCusto == precoCusto && PrecoVenda == precoVenda) return;
    ValidarPrecos(precoCusto, precoVenda);
    PrecoCusto = precoCusto;
    PrecoVenda = precoVenda;
  }

  public void AlterarPlaca(string novaPlaca, bool placaJaExisteEmOutroVeiculo)
  {
    if (Placa == novaPlaca) return;

    if (placaJaExisteEmOutroVeiculo)
      throw new DomainException("Esta placa já existe em outro veículo");

    ValidarPlaca(novaPlaca);
    Placa = novaPlaca;
  }

  public void Desativar()
  {
    Ativo = false;
  }

  // Validações Privadas (Encapsulamento)
  private void ValidarPlaca(string placa)
  {
    if (string.IsNullOrWhiteSpace(placa) || placa.Length != 7)
      throw new DomainException("Formato de placa inválido. Deve conter 7 caracteres.");
  }

  private void ValidarDados(int ano)
  {
    if (ano < 1900 || ano > DateTime.Now.Year + 1)
      throw new DomainException("Ano do veículo inválido.");
  }

  // Nova blindagem de negócio 
  private void ValidarPrecos(decimal precoCusto, decimal precoVenda)
  {
    if (precoCusto < 0)
      throw new DomainException("O preço de custo não pode ser negativo.");

    if (precoVenda < precoCusto)
      throw new DomainException("Prejuízo detectado: O preço de venda não pode ser menor que o preço de custo.");
  }
}