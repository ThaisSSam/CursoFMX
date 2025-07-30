using System;

namespace exercicio.Classe;

public class SmsNotificador : NotificadorBase, INotificavel
{
    public string NumeroTelefone { get; set; }

    public SmsNotificador(string Remetente, string NumeroTelefone)
    {
        Remetente = Remetente;
        NumeroTelefone = NumeroTelefone;
    }

    override void ConfigurarCredenciais()
    {
        Console.WriteLine("Configurando credenciais de SMS...");
    }

    void EnviarNotificacao(string mensagem)
    {
        Console.WriteLine($"SMS de {Remetente} para {NumeroTelefone}: {mensagem}");
        LogarNotificacao(mensagem);
    }
}