using System;
using LojaApi.Entities;
using LojaApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LojaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<Produto>> GetAll()
        {
            var produtos = ProdutoRepository.GetAll();
            // 200 OK - Sucesso 
            return Ok(produtos);
        }

        [HttpGet("{id}")]
        public ActionResult<Produto> GetById(int id)
        {
            var produto = ProdutoRepository.GetById(id);

            if (produto == null)
            {
                // 404 Not Found - Recurso não encontrado 
                return NotFound();
            }

            // 200 OK - Sucesso 
            return Ok(produto);
        }

        [HttpPost]
        public ActionResult<Produto> Add(Produto novoProduto)
        {
            // Validação simples (o ideal é fazer validações mais complexas) 
            if (string.IsNullOrWhiteSpace(novoProduto.Nome))
            {
                // 400 Bad Request - Erro no produto (dados inválidos) 
                return BadRequest("O nome do produto é obrigatório.");
            }

            var produtoCriado = ProdutoRepository.Add(novoProduto);

            // 201 Created - Novo recurso criado com sucesso 
            // Retorna o recurso criado e a URL para acessá-lo (boa prática REST) 
            return CreatedAtAction(nameof(GetById), new { id = produtoCriado.Id }, produtoCriado);
        }

        // Endpoint: PUT api/Produtos/{id} 
        [HttpPut("{id}")]
        public ActionResult<Produto> Update(int id, Produto produtoAtualizado)
        {
            // Validação de entrada 
            if (string.IsNullOrWhiteSpace(produtoAtualizado.Nome))
            {
                return BadRequest("O nome do produto é obrigatório para atualização.");
            }

            var produto = ProdutoRepository.Update(id, produtoAtualizado);

            if (produto == null)
            {
                // 404 Not Found - Tentou atualizar algo que não existe 
                return NotFound();
            }

            // 200 OK - Sucesso (Recurso substituído) 
            return Ok(produto);
        } 

        // Endpoint: DELETE api/Produtos/{id} 
        [HttpDelete("{id}")] 
        public IActionResult Delete(int id) // Usamos IActionResult pois não retornaremos um objeto Produto 
        { 
            var sucesso = ProdutoRepository.Delete(id); 

            if (!sucesso) 
            { 
                // 404 Not Found - Tentou deletar algo que não existe 
                return NotFound(); 
            } 

            // 204 No Content - Sucesso, mas não há corpo de resposta (padrão REST para DELETE) 
            return NoContent();  
        } 
    }
}