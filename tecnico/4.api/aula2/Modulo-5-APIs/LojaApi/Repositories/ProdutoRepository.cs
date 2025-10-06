using System;
using LojaApi.Entities;

namespace LojaApi.Repositories
{
    public class ProdutoRepository
    {
        private static List<Produto> _produtos = new List<Produto>
        {
            new Produto {Id= 1, Nome = "Mouse", Valor= 50, Descricao= "Mouse com fio", Estoque= 10},
            new Produto {Id= 2, Nome = "Teclado", Valor= 80, Descricao= "Teclado com fio", Estoque= 15},
        };

        private static int _nextId = 4;

        // 1. Read (Ler Todos) 
        public static List<Produto> GetAll()
        {
            return _produtos;
        }

        // 2. Read (Ler por ID)
        public static Produto? GetById(int id)
        {
            // Retorna o primeiro produto com o ID, ou null se não encontrar 
            return _produtos.FirstOrDefault(p => p.Id == id);
        }

        // 3. Create (Criar) 
        public static Produto Add(Produto novoProduto)
        {
            novoProduto.Id = _nextId++; // Atribui o próximo ID 
            _produtos.Add(novoProduto);
            return novoProduto;
        }

        // 4. Update (Substituir/Completo) 
        public static Produto? Update(int id, Produto produtoAtualizado)
        {
            var produtoExistente = _produtos.FirstOrDefault(c => c.Id == id);

            if (produtoExistente == null)
            {
                return null; // Não encontrou para atualizar 
            }

            // O PUT (Update) substitui os campos 
            produtoExistente.Nome = produtoAtualizado.Nome;
            produtoExistente.Valor = produtoAtualizado.Valor;
            produtoExistente.Descricao = produtoAtualizado.Descricao;
            produtoExistente.Estoque = produtoAtualizado.Estoque; // Assume-se que todos os outros campos vieram

            return produtoExistente;
        }

        // 5. Delete (Excluir) 
        public static bool Delete(int id)
        {
            var clienteParaDeletar = _produtos.FirstOrDefault(c => c.Id == id);

            if (clienteParaDeletar == null)
            {
                return false; // Não encontrou para deletar 
            }

            _produtos.Remove(clienteParaDeletar);
            return true;
        }
    }
}