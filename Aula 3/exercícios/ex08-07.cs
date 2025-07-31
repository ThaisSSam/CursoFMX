// Objetivo
// Criar um programa que valida a estrutura de um CPF e o formata para exibição.

// Descrição
// O programa deve solicitar ao usuário que digite um número de CPF. Em seguida, você deve criar os seguintes métodos:

// Um método ValidarCpf que recebe uma string (o CPF) e retorna um bool indicando se ele possui 11 dígitos e se todos são números.

// Um método FormatarCpf que recebe uma string (o CPF válido) e retorna uma nova string com o CPF formatado no padrão "XXX.XXX.XXX-XX".

// No método Main, chame esses métodos. Se o CPF for inválido, exiba uma mensagem de erro. Se for válido, exiba o CPF formatado.

// Regras de validação simples para este exercício:

// O CPF deve ter exatamente 11 caracteres.

// Todos os caracteres devem ser dígitos numéricos.

// Exemplo de interação:

// Entrada inválida:

// Digite o CPF (apenas números): 12345
// CPF inválido! O CPF deve conter 11 dígitos numéricos.
// Entrada válida:

// Digite o CPF (apenas números): 12345678901
// CPF formatado: 123.456.789-01

class Program
{
    public static bool ValidarCpf(string cpf)
    {
        if (cpf.Length != 11)
        {
            return false;
        }
        else
        {
            foreach (char c in cpf)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }
            return true;
        }
    }

    public static string FormatarCpf(string cpfValido)
    {
         return $"{cpfValido.Substring(0, 3)}.{cpfValido.Substring(3, 3)}.{cpfValido.Substring(6, 3)}-{cpfValido.Substring(9, 2)}";
    }
    
    static void Main(string[] args)
    {
        Console.WriteLine("Digite o CPF:");
        string cpf = Console.ReadLine();

        if (ValidarCpf(cpf) == true)
        {
           Console.WriteLine(FormatarCpf(cpf));
        }
        else
        {
            Console.WriteLine("CPF inválido! O CPF deve conter 11 dígitos numéricos.");
        }
    }
}