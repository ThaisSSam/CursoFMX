using System;
using LojaApi.Entities;

namespace LojaApi.Repositories
{
    public class ProdutoRepository
    {
        private static List<Produto> produtos = new List<Produto>
        {
            new Produto {Id= 1, Nome = "Mouse", Valor= 50, Descricao= "Mouse com fio", Estoque= 10},
            new Produto {Id= 2, Nome = "Teclado", Valor= 80, Descricao= "Teclado com fio", Estoque= 15},
        };

        private static int _nextId = 4;

        public static List<Produto> GetAll()
        {
            return produtos;
        }

        public static Produto? GetById(int id)
        {
            // Retorna o primeiro cliente com o ID, ou null se não encontrar 
            return produtos.FirstOrDefault(p => p.Id == id);
        }

        public static Produto Add(Produto novoProduto)
        {
            novoProduto.Id = _nextId++; // Atribui o próximo ID 
            produtos.Add(novoProduto);
            return novoProduto;
        }
    }
}