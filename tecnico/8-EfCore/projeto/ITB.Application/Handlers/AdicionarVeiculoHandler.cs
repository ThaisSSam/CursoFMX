// using System.Security.Claims;
// using ITB.Application.Commands;
// using ITB.Domain.Core.Messages;
// using ITB.Domain.Core.Messages.Interfaces;
// using ITB.Domain.Entities;
// using ITB.Domain.Interfaces;
// using Microsoft.AspNetCore.Http;

// namespace ITB.Application.Handlers;

// public class AdicionarVeiculoHandler : IHandler<AdicionarVeiculoCommand>
// {
//     private readonly IHttpContextAccessor _httpContextAccesor;
//     private IVeiculoRepository _repository;
//     private IUnitOfWork _uow;
//     public AdicionarVeiculoHandler(IHttpContextAccessor httpContextAcessor, IVeiculoRepository repository, IUnitOfWork uow)
//     {
//         _repository = repository;
//         _uow = uow;
//         _httpContextAccesor = httpContextAcessor;
//     }

//     public async Task Handle(AdicionarVeiculoCommand comando)
//     {

//         var usuario = _httpContextAccesor.HttpContext?.User;
//         var usuarioIdString = usuario?.FindFirstValue(ClaimTypes.NameIdentifier);
//         _ = int.TryParse(usuarioIdString, out int usuarioId);
//         var nomeUsuario = usuario?.FindFirstValue(ClaimTypes.Name);
//         // comando.CriadoPorId = 
//         Console.WriteLine($"O veículo está sendo cadastro pelo usuário: {nomeUsuario}");

//         var veiculo = new Veiculo(comando.Nome, comando.ModeloId, comando.Placa, comando.Ano, comando.MarcaId, comando.PrecoCusto, comando.PrecoVenda);
//         await _repository.AdicionarAsync(veiculo);
//         await _uow.Commit();

//         // return new CommandResult(
//         //     sucesso: true,
//         //     mensagem: "Veículo adicionado com sucesso",
//         //     dados: new
//         //     {
//         //         Id = veiculo.Id
//         //     }
//         // );
//     }
// }
using ITB.Application.Commands;
using ITB.Domain.Core.Messages;
using ITB.Domain.Core.Messages.Interfaces;
using ITB.Domain.Entities;
using ITB.Domain.Interfaces;

public class AdicionarVeiculoHandler : IHandler<AdicionarVeiculoCommand>
{
    private readonly IVeiculoRepository _veiculoRepository;
    private readonly IMarcaRepository _marcaRepository;
    private readonly IUnitOfWork _uow;
    
    // Injetamos a inteligência da nova arquitetura: O Bloco de Notas!
    private readonly IDomainNotificationHandler<DomainNotification> _notifications;

    public AdicionarVeiculoHandler(IVeiculoRepository veiculoRepository, 
                                   IMarcaRepository marcaRepository, 
                                   IUnitOfWork uow,
                                   IDomainNotificationHandler<DomainNotification> notifications) // <-- Adicionado aqui
    {
        _veiculoRepository = veiculoRepository;
        _marcaRepository = marcaRepository;
        _uow = uow;
        _notifications = notifications;
    }

    public async Task Handle(AdicionarVeiculoCommand command)
    {
        // 1. Validação de Existência (Regra de Negócio)
        var marcaExiste = await _marcaRepository.VerificarExistencia(command.MarcaId);
        
        if (!marcaExiste)
        {
            // O FIM DAS EXCEPTIONS! Anotamos o erro e usamos o 'return' para parar a execução na hora.
            await _notifications.Handle(new DomainNotification("MarcaId", "A marca informada não existe no sistema."));
            return; 
        }

        // 2. Criação do Domínio Rico 
        var veiculo = new Veiculo(command.Nome, command.ModeloId, command.Placa, command.Ano, command.MarcaId, command.PrecoCusto, command.PrecoVenda);

        // 3. Persistência
        await _veiculoRepository.AdicionarAsync(veiculo);

        if (!await _uow.Commit())
        {
            // Ocorreu um problema ao salvar. Anotamos também e paramos.
            await _notifications.Handle(new DomainNotification("BancoDeDados", "Ocorreu um erro ao salvar as alterações."));
            return;
        }

        // 4. O Retorno Inteligente! 
        // Como o banco acabou de gerar o ID (após o Commit), nós injetamos esse ID dentro do Command.
        // A Controller terá acesso a esse objeto e poderá ler o ID!
        command.IdGerado = veiculo.Id;
    }
}