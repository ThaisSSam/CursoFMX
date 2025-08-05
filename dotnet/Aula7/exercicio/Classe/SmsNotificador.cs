using System;
using exercicio.ClasseAbstrata;
using exercicio.Interface;

namespace exercicio.Classe
{
    public class SmsNotificador : NotificadorBase, INotificavel
    {
        public string NumeroTelefone { get; set; }

        public SmsNotificador(string remetente, string numeroTelefone)
            : base(remetente)
        {
            NumeroTelefone = numeroTelefone;
        }

        public override void ConfigurarCredenciais()
        {
            Console.WriteLine("Configurando credenciais de SMS...");
        }

        public void EnviarNotificacao(string mensagem)
        {
            Console.WriteLine($"SMS de {Remetente} para {NumeroTelefone}: {mensagem}");
            LogarNotificacao(mensagem);
        }
    }
}
