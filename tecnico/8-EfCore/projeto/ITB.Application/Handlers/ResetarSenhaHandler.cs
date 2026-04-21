using System;
using ITB.Application.Commands;
using ITB.Domain.Core.Messages;
using ITB.Domain.Core.Messages.Interfaces;
using ITB.Domain.Interfaces;

namespace ITB.Application.Handlers;

public class ResetarSenhaHandler(
    IUsuarioReadRepository readRepository,
    IUsuarioWriteRepository writeRepository,
    IUnitOfWork uow,
    IDomainNotificationHandler<DomainNotification> notifications) 
    : IHandler<ResetarSenhaCommand>
{
    public async Task Handle(ResetarSenhaCommand command)
    {
        // 1. Busca o usuário pelo E-mail fornecido
        var usuario = await readRepository.ObterPorEmailAsync(command.Email);

        // Segurança: Em sistemas em produção nós não avisamos se o e-mail não existe,
        // para não dar dicas a hackers. Mas no nosso escopo didático corporativo, vamos avisar.
        if (usuario == null)
        {
            await notifications.Handle(new DomainNotification("Email", "E-mail não encontrado na base de dados."));
            return;
        }

        // 2. Aciona o Domínio Rico: A Entidade gera a senha, criptografa e ativa a trava de segurança
        var senhaGeradaPeloDominio = usuario.ResetarSenha();

        // 3. Prepara para atualizar o Hash e a Trava no banco
        writeRepository.Atualizar(usuario);
        
        if (await uow.Commit())
        { 
            // Se o banco salvou com sucesso, nós colocamos a senha gerada dentro do Command.
            // Assim, a Controller consegue pegar esse valor e enviar na tela ou por E-mail!
            command.SenhaProvisoriaGerada = senhaGeradaPeloDominio;
        }
        else
        {
            await notifications.Handle(new DomainNotification("Banco", "Houve um erro ao processar o reset da senha."));
        }
    }
}
