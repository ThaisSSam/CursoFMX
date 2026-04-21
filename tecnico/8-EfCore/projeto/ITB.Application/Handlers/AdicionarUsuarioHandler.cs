using System;
using ITB.Application.Commands;
using ITB.Domain.Core.Messages;
using ITB.Domain.Core.Messages.Interfaces;
using ITB.Domain.Entities;
using ITB.Domain.Interfaces;

namespace ITB.Application.Handlers;

// O PRESENTE (C# 12): Direto ao ponto! 
public class AdicionarUsuarioHandler(
    IUsuarioWriteRepository writeRepository,
    IUsuarioReadRepository readRepository,
    IUnitOfWork uow,
    IDomainNotificationHandler<DomainNotification> notifications)
    : IHandler<AdicionarUsuarioCommand>
{
  public async Task Handle(AdicionarUsuarioCommand command)
  {
    // 1. Validação de E-mail Único (Regra de Negócio Crítica)
    var usuarioExistente = await readRepository.ObterPorEmailAsync(command.Email);

    if (usuarioExistente != null)
    {
      await notifications.Handle(new DomainNotification("Email", "Este e-mail já está em uso no sistema."));
      return;
    }

    // 2. Acionando a Fábrica da Entidade (Result Pattern)
    var resultadoCriacao = Usuario.Criar(command.Nome, command.Email, command.Senha, command.Perfil);

    // 3. Verifica se a Entidade rejeitou os dados
    if (!resultadoCriacao.IsSuccess)
    {
      foreach (var erro in resultadoCriacao.Errors)
      {
        await notifications.Handle(new DomainNotification("Usuario", erro));
      }
      return; // Fail-Fast
    }

    // 4. Se chegou aqui, a entidade é válida e a senha já está criptografada
    var novoUsuario = resultadoCriacao.Value;

    // 5. Persistência
    await writeRepository.AdicionarAsync(novoUsuario!);

    if (await uow.Commit())
    {
      command.IdGerado = novoUsuario!.Id;
    }
    else
    {
      await notifications.Handle(new DomainNotification("Banco", "Houve um erro ao salvar o usuário no banco de dados."));
    }
  }
}