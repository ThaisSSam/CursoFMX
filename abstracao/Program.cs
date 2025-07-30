using abstracao;
using abstracao.Classes;
// using abstracao.ClassesAbstratas;
using abstracao.Interfaces;

Console.WriteLine("--- Demonstrando Interfaces ---");

// Criando objetos de classes que implementam IImprimivel
Documento relatorio = new Documento("Relatório de Vendas", "Vendas do mês de Julho...");

Foto selfie = new Foto("minha_selfie.jpg", "1920x1080");

// Podemos colocar objetos de tipos diferentes em uma lista da Interface!
List<IImprimivel> filaDeImpressao = new List<IImprimivel>();
filaDeImpressao.Add(relatorio);
filaDeImpressao.Add(selfie);

Console.WriteLine("\n--- Fila de Impressão (Polimorfismo!) ---");
foreach (var item in filaDeImpressao)
{
    item.Imprimir(); // Chama o método Imprimir() de cada item polimorficamente
    Console.WriteLine($"Conteúdo para pré-visualização: {item.ObterConteudoImpressao}...");
}
Console.WriteLine("\n-------------------------------------");

// Podemos verificar se um objeto implementa uma interface
if (relatorio is IImprimivel)
{
    Console.WriteLine("Relatório implementa IImprimivel.");
}



Console.WriteLine("\n--- Demonstrando Classes Abstratas ---");

// NÃO PODE: FormaGeometrica formaGenerica = new FormaGeometrica("verde"); // ERRO!
// Uma classe abstrata não pode ser instanciada diretamente.

// Podemos instanciar as classes CONCRETAS (que não são abstratas)
Circulo circuloVermelho = new Circulo("Vermelho", 5.0);
Retangulo retanguloAzul = new Retangulo("Azul", 10.0, 4.0);

// Polimorfismo: Podemos criar uma lista do tipo da classe abstrata!
List<FormaGeometrica> minhasFormas = new List<FormaGeometrica>();
minhasFormas.Add(circuloVermelho);
minhasFormas.Add(retanguloAzul);

Console.WriteLine("\n--- Calculando Áreas (Polimorfismo com Abstração!) ---");
foreach (var forma in minhasFormas)
{
    forma.ExibirCor(); // Método concreto herdado de FormaGeometrica
    Console.WriteLine($"Área calculada: {forma.CalcularArea():F2}"); // Método abstrato sobrescrito (polimorfismo)
}