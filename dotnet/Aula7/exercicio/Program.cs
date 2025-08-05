using System;
using System.Collections.Generic;
using exercicio.Classe;           
using exercicio.Interface;       
using exercicio.ClasseAbstrata;  

class Program
{
    static void Main()
    {
        List<INotificavel> Lista = new List<INotificavel>();

        EmailNotificador emailNotificador = new EmailNotificador("sistema@empresa.com", "cliente@email.com");
        SmsNotificador smsNotificador = new SmsNotificador("Sistema","(99)99999-9999");

        Lista.Add(emailNotificador);
        Lista.Add(smsNotificador);

        foreach (var item in Lista)
        {
            if (item is NotificadorBase notificador)
            {
                notificador.ConfigurarCredenciais();
            }

            item.EnviarNotificacao("Mensagem de teste!");
        }
    }
}
