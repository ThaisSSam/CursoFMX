using System;

namespace LojaApi.Entities{

    public class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public decimal Valor { get; set; } = 0.0m;
        public string Descricao { get; set; } = string.Empty;
        public int Estoque { get; set; }
    }
}