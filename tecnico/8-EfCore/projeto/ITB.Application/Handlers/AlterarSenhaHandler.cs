using System;
using ITB.Application.Commands;
using ITB.Domain.Core.Messages;
using ITB.Domain.Core.Messages.Interfaces;
using ITB.Domain.Interfaces;

namespace ITB.Application.Handlers;

public class AlterarSenhaHandler(
    IUsuarioReadRepository readRepository,
    IUsuarioWriteRepository writeRepository,
    IUnitOfWork uow,
    IDomainNotificationHandler<DomainNotification> notifications)
    : IHandler<AlterarSenhaCommand>
{
  public async Task Handle(AlterarSenhaCommand command)
  {
    // 1. Busca o usuário no banco (Usando o repositório de leitura) 
    var usuario = await readRepository.ObterPorIdAsync(command.UsuarioId);

    if (usuario == null)
    {
      await notifications.Handle(new DomainNotification("Usuario", "Usuário não localizado no sistema."));
      return;
    }

    // 2. Aciona o Domínio Rico: A entidade tenta alterar a própria senha 
    var resultado = usuario.AlterarSenha(command.SenhaAtual, command.NovaSenha);

    // 3. Se a entidade rejeitou a ação (senha atual errada, ou nova muito curta),  
    // transferimos a culpa para o Bloco de Notas 
    if (!resultado.IsSuccess)
    {
      foreach (var erro in resultado.Errors)
      {
        await notifications.Handle(new DomainNotification("Senha", erro));
      }
      return; // Fail-Fast silencioso e performático! 
    }

    // 4. Se a entidade aceitou, preparamos para salvar 
    writeRepository.Atualizar(usuario);


    // 5. Commit no banco de dados 
    if (!await uow.Commit())
    {
      await notifications.Handle(new DomainNotification("Banco", "Houve um erro ao salvar a nova senha no banco de dados."));
    }
  }
}