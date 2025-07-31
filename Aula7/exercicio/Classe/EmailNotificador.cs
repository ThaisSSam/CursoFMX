using System;
using exercicio.ClasseAbstrata;
using exercicio.Interface;

namespace exercicio.Classe
{
    public class EmailNotificador : NotificadorBase, INotificavel
    {
        public string EmailDestino { get; set; }

        public EmailNotificador(string remetente, string emailDestino)
            : base(remetente)
        {
            EmailDestino = emailDestino;
        }

        public override void ConfigurarCredenciais()
        {
            Console.WriteLine("Configurando credenciais de e-mail...");
        }

        public void EnviarNotificacao(string mensagem)
        {
            Console.WriteLine($"Email de {Remetente} para {EmailDestino}: {mensagem}");
            LogarNotificacao(mensagem);
        }
    }
}
