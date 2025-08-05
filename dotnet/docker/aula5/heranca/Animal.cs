using System;

namespace aula5;

public class Animal 
{ 
    public string Nome { get; set; } 
    public int Idade { get; set; } 
    protected string Especie { get; set; } // Protected: acessível pela própria classe e classes derivadas 
 
    public Animal(string nome, int idade, string especie) 
    { 
        Nome = nome; 
        Idade = idade; 
        Especie = especie; 
        Console.WriteLine($"\nConstrutor Animal: '{Nome}' ({Especie}) criado."); 
    } 
 
    public void Comer() 
    { 
        Console.WriteLine($"{Nome} está comendo."); 
    } 
 
    public virtual void EmitirSom() // 'virtual' permite que classes derivadas modifiquem este comportamento (Polimorfismo!) 
    { 
        Console.WriteLine($"{Nome} está emitindo um som genérico."); 
    } 
} 

 