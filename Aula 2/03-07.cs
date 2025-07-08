// // 
using System.Net.Http.Headers;
using System.Runtime.InteropServices;

Console.WriteLine("03/07/25 - repetição");
// int num1 = 9;
// int num2 = 2;

// double divisaoReal = (double)num1 / num2;
// Console.WriteLine($"Divisão Real (double): {divisaoReal}");

// double divisaoSemDouble = num1 / num2;
// Console.WriteLine($"Divisão Real (double): {divisaoSemDouble}");

// shift+ alt+ f -> indentação

// *Array* 
// Console.WriteLine($"\n-----ARRAY-----");
// string[] nomesAlunos = new string[3];

// nomesAlunos[0] = "Maria";
// nomesAlunos[2] = "Pedro";

// int[] idades = new int[5] { 16, 20, 24, 14, 19 };

// // Console.WriteLine($"Aluno 3: {nomesAlunos[2]}");

// // // utilizar constante para números esporadicos
// // const int DIAS_DA_SEMANA = 7;


// // *Lista*
// // Console.WriteLine($"\n-----LISTAS-----");
// List<string> listaNomes = new List<string>();

// // Adiciona no final da lista
// listaNomes.Add("Carlos");
// listaNomes.Add("Ana");
// listaNomes.Add("Bruno");

// // Console.WriteLine($"Lista: {string.Join(", ", listaNomes)}");

// listaNomes.Remove("Carlos");
// listaNomes.Add("Thais");
// listaNomes.Add("João");

// Console.WriteLine($"Lista: {string.Join(", ", listaNomes)}");
// Console.WriteLine($"Elementos: {listaNomes.Count}");

// *For*
// Console.WriteLine($"\n-----FOR-----");
// for (int i = 0; i < idades.Length; i++)
// {
//     Console.WriteLine($"Idade {i + 1}: {idades[i]}");
// }

// for (int i = 0; i < listaNomes.Count; i++)
// {
//     Console.WriteLine($" - {listaNomes[i]}");
// }

// *ForEach*
// foreach (double idade in idades)
// {
//     Console.WriteLine($"Idade {idade}");
// }

// // Oedenando
// Console.WriteLine($"\n-----Ordenação-----");
// listaNomes.Sort();
// Console.WriteLine($"Lista: {string.Join(", ", listaNomes)}");

// juntar listas
// nomesAlunos

// Console.WriteLine($"\n-----Controle de lista-----");
// // Fzr nova lista de outra
// List<int> maioresDeIdade = idades.Where(idade => idade >= 18).ToList();
// Console.WriteLine($"Lista: {string.Join(", ", maioresDeIdade)}");

// Console.WriteLine($"Média de idades: {maioresDeIdade.Average()}");



Console.WriteLine("Exercício 1: Gerenciamento Simples de Notas");
// Você é um professor e precisa gerenciar as notas de 5 alunos em uma disciplina.

// Crie um Array para armazenar as 5 notas.

// Preencha esse Array com notas de sua escolha (entre 0 e 10).

// Use um loop for para exibir todas as notas e o índice de cada uma (ex: "Nota do Aluno 1: 7.5").

// Use um loop foreach para calcular e exibir a média das notas.


List<string> listaAlunos = new List<string>();
listaAlunos.Add("Maria");
listaAlunos.Add("Thais");
listaAlunos.Add("João");
listaAlunos.Add("Ana");
listaAlunos.Add("Bruno");

double[] notas = new double[5] { 2.3, 5.5, 7.7, 9.1, 10 };
double media = 0;

for (int i=0; i< notas.Length; i++)
{
    Console.WriteLine($"Nota do {listaAlunos[i]}: {notas[i]}");
}

foreach (double nota in notas)
{
    double soma = notas.Sum();
    media = (double)soma / 5;
}
Console.WriteLine($"Média: {media}");

// Console.WriteLine($"Calculo:  {string.Join("+ ", notas)} /5");
// double media = notas.Average();
// Console.WriteLine($"Média: {media}");