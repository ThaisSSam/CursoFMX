using System;

namespace exercicio.Classe;

public class EmailNotification : NotificadorBase, INotificavel
{
    public string EmailDestino { get; set; }
    public EmailNotificador(string Remetente, string EmailDestino)
    {
        Remetente = Remetente;
        EmailDestino = EmailDestino;
    }

    override void ConfigurarCredenciais()
    {
        Console.WriteLine("Configurando credenciais de e-mail...");
    }

    void EnviarNotificacao(string mensagem)
    {
        Console.WriteLine($"Email de {Remetente} para {EmailDestino}: {mensagem}");
        LogarNotificacao(mensagem);
    }
}