namespace exercicio.ClasseAbstrata
{
    public abstract class NotificadorBase
    {
        public string Remetente { get; set; }

        protected NotificadorBase(string remetente)
        {
            Remetente = remetente;
        }

        public void LogarNotificacao(string mensagem)
        {
            Console.WriteLine($"Log: {mensagem}");
        }

        public abstract void ConfigurarCredenciais();
    }
}
