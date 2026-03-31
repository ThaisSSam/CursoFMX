using System;
using System.Security.Claims;
using ITB.Application.Commands;
using ITB.Domain.Core.Messages;
using ITB.Domain.Core.Messages.Interfaces;
using ITB.Domain.Entities;
using ITB.Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace ITB.Application.Handlers;

public class AdicionarVeiculoHandler : IHandler<AdicionarVeiculoCommand>
{
    private readonly IHttpContextAccessor _httpContestAccessor;
    private readonly IVeiculoRepository _veiculoRepository;

    private readonly IUnitOfWork _uow;

    // public AdicionarVeiculoHandler(IVeiculoRepository veiculoRepository) => _veiculoRepository = veiculoRepository;

    public AdicionarVeiculoHandler(IVeiculoRepository veiculoRepository, IUnitOfWork uow)
    {
        _veiculoRepository = veiculoRepository;
        _uow = uow;
    }
    

    // public async Task Handle(AdicionarVeiculoCommand command)
    // {
    //     var veiculo = new Veiculo(
    //         command.placa, 
    //         command.ano, 
    //         command.modeloId
    //     );
    //     await _veiculoRepository.AdicionarAsync(veiculo);
    // }

    public async Task<CommandResult> Handle(AdicionarVeiculoCommand comando)
    {
        // 1. Acessa o usuário logado que enviou a requisição 
        var usuario = _httpContestAccessor.HttpContext?.User; 
 
        // 2. Procura pela Claim 'Sub' (NameIdentifier) que nós gravamos no Login! 
        var usuarioIdString = usuario?.FindFirstValue(ClaimTypes.NameIdentifier); 
        
        // Converte para inteiro (já que salvamos o ID "99" no AuthController) 
        _ = int.TryParse(usuarioIdString, out int usuarioId); 

        // 3. Procura pelo Nome do usuário 
        var nomeUsuario = usuario?.FindFirstValue(ClaimTypes.Name); 

        // 4. Agora podemos usar esses dados para auditoria! 
        // request.CriadoPorId = usuarioId; // Grava no banco: Criado pelo Usuário 99 
        
        Console.WriteLine($"O veículo está sendo cadastrado pelo usuário: {nomeUsuario}");

        var novaVeiculo = new Veiculo(comando.id, comando.placa);
        _veiculoRepository.AdicionarAsync(novaVeiculo);
        await _uow.CommitAsync();

        return new CommandResult(
            sucesso: true,
            mensagem: "Veiculo cadastrada com sucesso!",
            dados: new {id = novaVeiculo.Id}
        );
    }
}
