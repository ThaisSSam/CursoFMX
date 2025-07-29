using System;

namespace exercicio.ClasseAbstrata;

public class NotificadorBase
{
    public string Remetente { get; set; }
    protected NotificadorBase(string remetente);

    public void LogarNotificacao(string mensagem);

    public abstract void ConfigurarCredenciais();
}