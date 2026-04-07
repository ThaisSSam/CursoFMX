using System;
using System.Security.Claims;
using System.Threading.Tasks; // Adicionado para o Task
using ITB.Application.Commands;
using ITB.Domain.Core.Messages;
using ITB.Domain.Core.Messages.Interfaces;
using ITB.Domain.Core.Notifications;
using ITB.Domain.Entities;
using ITB.Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace ITB.Application.Handlers;

// public class AdicionarVeiculoHandler : IHandler<AdicionarVeiculoCommand>
// {
//     private readonly IHttpContextAccessor _httpContextAccessor; // Corrigido nome e tipo
//     private readonly IVeiculoRepository _veiculoRepository;
//     private readonly IUnitOfWork _uow;

//     // 1. O segredo: Você precisa injetar o IHttpContextAccessor no construtor!
//     public AdicionarVeiculoHandler(
//         IVeiculoRepository veiculoRepository, 
//         IUnitOfWork uow, 
//         IHttpContextAccessor httpContextAccessor)
//     {
//         _veiculoRepository = veiculoRepository;
//         _uow = uow;
//         _httpContextAccessor = httpContextAccessor;
//     }

//     public async Task<CommandResult> Handle(AdicionarVeiculoCommand comando)
//     {
//         // 1. Acessa o usuário logado (O Token que você colou no Authorize do Swagger)
//         var usuario = _httpContextAccessor.HttpContext?.User; 
 
//         // 2. Procura pelas Claims que gravamos no TokenService
//         var usuarioIdString = usuario?.FindFirstValue(ClaimTypes.NameIdentifier); 
//         var nomeUsuario = usuario?.FindFirstValue(ClaimTypes.Name); 

//         // Tenta converter o ID (útil para auditoria no banco)
//         int.TryParse(usuarioIdString, out int usuarioId); 
        
//         Console.WriteLine($"O veículo está sendo cadastrado pelo usuário: {nomeUsuario} (ID: {usuarioId})");

//         // 3. Criando a entidade Veiculo com os dados do comando
//         // Use os nomes das propriedades que estão no seu AdicionarVeiculoCommand
//         var novoVeiculo = new Veiculo(
//             comando.placa, 
//             comando.ano, 
//             comando.modeloId,
//             comando.precoCusto,
//             comando.precoVenda
//         );

//         // 4. Salva no Repositório e faz o Commit no Banco
//         await _veiculoRepository.AdicionarAsync(novoVeiculo);
//         await _uow.CommitAsync();

//         return new CommandResult(
//             sucesso: true,
//             mensagem: "Veículo cadastrado com sucesso!",
//             dados: new { id = novoVeiculo.Id }
//         );
//     }
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
        var marcaExiste = await _marcaRepository.VerificarExistencia(command.marcaId);
        
        if (!marcaExiste)
        {
            // O FIM DAS EXCEPTIONS! Anotamos o erro e usamos o 'return' para parar a execução na hora.
            await _notifications.Handle(new DomainNotification("MarcaId", "A marca informada não existe no sistema."));
            return; 
        }

        // 2. Criação do Domínio Rico 
        var veiculo = new Veiculo(command.placa, command.ano, command.modeloId, command.precoCusto, command.precoVenda, command.marcaId);

        // 3. Persistência
        await _veiculoRepository.AdicionarAsync(veiculo);

        if (!await _uow.CommitAsync())
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