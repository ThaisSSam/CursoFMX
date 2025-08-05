using System;
using abstracao.Interfaces;

namespace abstracao.Classes;

public class Foto : IImprimivel // Implementa a interface
    {
        public string NomeArquivo { get; set; }
        public string Resolucao { get; set; }

        public Foto(string nomeArquivo, string resolucao)
        {
            NomeArquivo = nomeArquivo;
            Resolucao = resolucao;
        }

        // Implementação do método da interface
        public void Imprimir()
        {
            Console.WriteLine($"\n--- Imprimindo Foto: {NomeArquivo} ---");
            Console.WriteLine($"Resolução: {Resolucao}");
            Console.WriteLine("---------------------------------");
        }

        // Implementação da propriedade da interface
        public string ObterConteudoImpressao
        {
            get { return $"Foto: {NomeArquivo} ({Resolucao})"; }
        }
    }
 