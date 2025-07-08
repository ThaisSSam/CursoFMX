// See https://aka.ms/new-console-template for more information
Console.WriteLine("08/07/25 - métodos");
class aula {
    public static void MeuNovoMetodo()
    {
        Console.WriteLine("primeiro método");
    }

    public static void ExibirMensagemPersonalizada(string mensagem, int repeticoes)
    {
        for (int i = 0; i < repeticoes; i++)
        {
            Console.WriteLine(mensagem);
        }
    }

    public static int DobrarComReferencia(ref Numero) {
        Numero = Numero * 2;
    }

    static void Main(string[] args)
    {
        string Teste = "teste de método";
        int NumeroRepeticoes = 3;

        ExibirMensagemPersonalizada(Teste, NumeroRepeticoes);

        int Numero = 3;
        Console.WriteLine($"antes do método: {Numero}");
        Console.WriteLine($"método : {DobrarComReferencia}");
        Console.WriteLine($"depois do método: {Numero}");
    }

}

