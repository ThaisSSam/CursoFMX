using ITB.Domain.Core.Exceptions;

namespace ITB.Domain.Entities;

public class Veiculo
{
    public int Id { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public string Placa { get; private set; } = string.Empty;
    public int Ano { get; private set; }
    public int MarcaId { get; private set; }
    public virtual Marca Marca { get; private set; }
    public int ModeloId { get; private set; }
    public virtual Modelo Modelo { get; private set; }
    public bool Ativo { get; private set; } = true;
    public decimal PrecoCusto { get; private set; } = decimal.Zero;
    public decimal PrecoVenda { get; private set; } = decimal.Zero;
    // public uint ha { get; private set; }
    public Veiculo(string nome, int modeloId, string placa, int ano, int marcaId, decimal precoCusto, decimal precoVenda)
    {
        ValidarPlaca(placa);
        ValidarDados(nome, modeloId, ano);

        if (marcaId <= 0)
            throw new DomainException("Marca inválida.");

        Nome = nome;
        ModeloId = modeloId;
        Placa = placa;
        Ano = ano;
        MarcaId = marcaId;
        PrecoCusto = precoCusto;
        PrecoVenda = precoVenda;
    }
    
    public void AtualizarDados(int modeloId, int ano)
    {
        if (modeloId == ModeloId && ano == Ano) return;
        ModeloId = modeloId;
        Ano = ano;
    }

    public void AlterarPlaca(string novaPlaca, bool placaJaExisteEmOutroVeiculo)
    {
        if (Placa == novaPlaca) return;

        if (placaJaExisteEmOutroVeiculo)
            throw new DomainException("A placa já existe em outro veículo.");

        ValidarPlaca(novaPlaca);
        Placa = novaPlaca;
    }

    public void AlterarMarca(int novaMarcaId, bool novaMarcaExiste)
    {
        if (MarcaId == novaMarcaId) return;

        if (novaMarcaId <= 0)
            throw new DomainException("Marca inválida.");

        if (!novaMarcaExiste)
            throw new DomainException("A marca informada não existe.");

        MarcaId = novaMarcaId;
    }

    public void AlterarPrecos(decimal precoCusto, decimal precoVenda)
    {
        if (PrecoCusto == precoCusto && PrecoVenda == precoVenda) return;
        ValidarPrecos(precoCusto, precoVenda);
        PrecoCusto = precoCusto;
        PrecoVenda = precoVenda;
        // if (Placa == novaPlaca) return;

        // if (placaJaExisteEmOutroVeiculo)
        //     throw new DomainException("A placa já existe em outro veículo.");

        // ValidarPlaca(novaPlaca);
        // Placa = novaPlaca;
    }

    public void Desativar()
    {
        if (!Ativo) return;
        Ativo = false;
    }

    private void ValidarPlaca(string placa)
    {
        if (string.IsNullOrWhiteSpace(placa) || placa.Length < 7 || placa.Length > 8)
            throw new DomainException("Placa inválida.");
    }

    // Nova blindagem de negócio 
    private void ValidarPrecos(decimal precoCusto, decimal precoVenda) 
    { 
        if (precoCusto < 0)  
            throw new DomainException("O preço de custo não pode ser negativo."); 

        if (precoVenda < precoCusto)  
            throw new DomainException("Prejuízo detectado: O preço de venda não pode ser menor que o preço de custo.");
    } 

    private void ValidarDados(string nome, int modeloId, int ano)
    {
        if (modeloId <= 0)
            throw new DomainException("Modelo inválido.");

        if (ano < 1900 || ano > DateTime.Now.Year + 1)
            throw new DomainException("Ano inválido.");

        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException("Nome é obrigatório.");
    }
}