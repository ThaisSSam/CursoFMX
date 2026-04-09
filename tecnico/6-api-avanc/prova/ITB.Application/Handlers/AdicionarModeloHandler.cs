using System;
using System.Threading.Tasks;
using ITB.Application.Commands;
using ITB.Domain.Core.Notifications;
using ITB.Domain.Entities;
using ITB.Domain.Interfaces;
using ITB.Domain.Core.Messages.Interfaces;

namespace ITB.Application.Handlers;

public class AdicionarModeloHandler : IHandler<AdicionarModeloCommand>
{
    private readonly IModeloRepository _modeloRepository;
    private readonly IMarcaRepository _marcaRepository;
    private readonly IUnitOfWork _uow;
    private readonly IDomainNotificationHandler<DomainNotification> _notifications;

    public AdicionarModeloHandler(
        IModeloRepository modeloRepository, 
        IMarcaRepository marcaRepository,
        IUnitOfWork uow,
        IDomainNotificationHandler<DomainNotification> notifications)
    {
        _modeloRepository = modeloRepository;
        _marcaRepository = marcaRepository;
        _uow = uow;
        _notifications = notifications;
    }

    public async Task Handle(AdicionarModeloCommand comando)
    {
        // 1. Validação de Regra de Negócio: A Marca existe?
        var marcaExiste = await _marcaRepository.VerificarExistencia(comando.marcaId);
        
        if (!marcaExiste)
        {
            // Anotamos o erro no "bloco de notas" e paramos
            await _notifications.Handle(new DomainNotification("MarcaId", "A marca informada para este modelo não existe."));
            return; 
        }

        // 2. Criação da Entidade de Domínio
        // Certifique-se que o construtor do Modelo aceita (nome, marcaId, ativo)
        var novoModelo = new Modelo(comando.nome, comando.marcaId, comando.ativo);

        // 3. Persistência
        await _modeloRepository.AdicionarAsync(novoModelo);

        // 4. Commit e Verificação
        if (!await _uow.CommitAsync())
        {
            await _notifications.Handle(new DomainNotification("BancoDeDados", "Erro ao persistir o novo modelo."));
            return;
        }

        // 5. Retorno do ID gerado para o Command (para a Controller ler)
        comando.IdGerado = novoModelo.Id;
    }
}