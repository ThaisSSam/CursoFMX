using System;

using abstracao.Interfaces;

namespace abstracao.Classes;

public class Documento : IImprimivel // Implementa a interface
    {
        public string Titulo { get; set; }
        public string Conteudo { get; set; }

        public Documento(string titulo, string conteudo)
        {
            Titulo = titulo;
            Conteudo = conteudo;
        }

        // Implementação do método da interface
        public void Imprimir()
        {
            Console.WriteLine($"\n--- Imprimindo Documento: {Titulo} ---");
            Console.WriteLine(Conteudo);
            Console.WriteLine("-----------------------------------");
        }

        // Implementação da propriedade da interface
        public string ObterConteudoImpressao
        {
            get { return $"Documento: {Titulo}\n{Conteudo}"; }
        }
    }
 